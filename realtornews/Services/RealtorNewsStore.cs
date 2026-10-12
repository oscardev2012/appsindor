using Microsoft.Data.SqlClient;
using RealtorNews.Data;
using RealtorNews.Models;
using System.Data;

namespace RealtorNews.Services;

public sealed class RealtorNewsStore
{
    private readonly string _connectionString;

    public RealtorNewsStore(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("IndorDB")?.Trim() ?? "";
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_connectionString);

    public string? Probe()
    {
        if (!IsConfigured)
        {
            return "Falta ConnectionStrings:IndorDB.";
        }

        try
        {
            using var connection = Open();
            using var cmd = new SqlCommand("SELECT TOP (1) 1 FROM dbo.RealtorNews_Realtors", connection);
            cmd.ExecuteScalar();
            return null;
        }
        catch (SqlException ex) when (ex.Number is 208 or 207)
        {
            return "Crea las tablas nuevas con Data/realtor-news.sql y vuelve a recargar.";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public RealtorAccount? FindByEmail(string email)
    {
        return QueryRealtors("""
            SELECT TOP (1) r.*,
                (SELECT COUNT(*) FROM dbo.RealtorNews_Posts p WHERE p.RealtorId = r.Id AND p.IsPublished = 1) AS PostCount,
                (SELECT COUNT(*) FROM dbo.RealtorNews_Follows f WHERE f.FollowingId = r.Id) AS FollowerCount,
                (SELECT COUNT(*) FROM dbo.RealtorNews_Follows f WHERE f.FollowerId = r.Id) AS FollowingCount,
                CAST(0 AS bit) AS FollowedByMe
            FROM dbo.RealtorNews_Realtors r
            WHERE r.Email = @email
            """, cmd => cmd.Parameters.AddWithValue("@email", email.Trim().ToLowerInvariant()))
            .FirstOrDefault();
    }

    public RealtorAccount? FindById(Guid id)
        => QueryRealtors(RealtorSelect + " WHERE r.Id = @id", cmd => cmd.Parameters.AddWithValue("@id", id), null)
            .FirstOrDefault();

    public RealtorAccount? FindBySlug(string slug, Guid? viewerId = null)
        => QueryRealtors(RealtorSelect + " WHERE r.Slug = @slug", cmd => cmd.Parameters.AddWithValue("@slug", slug.Trim().ToLowerInvariant()), viewerId)
            .FirstOrDefault();

    public IReadOnlyList<RealtorAccount> Directory(Guid? viewerId = null)
        => QueryRealtors(RealtorSelect + " WHERE r.IsActive = 1 ORDER BY r.FullName", null, viewerId);

    public IReadOnlyList<RealtorAccount> Featured(int take = 4, Guid? viewerId = null)
        => QueryRealtors(RealtorSelect + " WHERE r.IsActive = 1 ORDER BY PostCount DESC, r.CreatedUtc", null, viewerId)
            .Take(take)
            .ToList();

    public RealtorAccount Register(RealtorAccount realtor)
    {
        realtor.Id = Guid.NewGuid();
        realtor.Email = realtor.Email.Trim().ToLowerInvariant();
        realtor.Slug = UniqueRealtorSlug(realtor.FullName);
        realtor.CreatedUtc = DateTimeOffset.UtcNow;

        using var connection = Open();
        using var cmd = new SqlCommand("""
            INSERT INTO dbo.RealtorNews_Realtors
                (Id, Email, PasswordHash, FullName, Slug, Headline, Bio, Phone, Brokerage, LicenseNumber,
                 City, State, PhotoUrl, WebsiteUrl, Languages, Specialties, IsActive, CreatedUtc)
            VALUES
                (@id, @email, @hash, @name, @slug, @headline, @bio, @phone, @brokerage, @license,
                 @city, @state, @photo, @web, @languages, @specialties, 1, @created)
            """, connection);
        BindRealtor(cmd, realtor);
        cmd.ExecuteNonQuery();
        return realtor;
    }

    public void UpdateProfile(RealtorAccount realtor)
    {
        using var connection = Open();
        using var cmd = new SqlCommand("""
            UPDATE dbo.RealtorNews_Realtors
            SET FullName = @name, Headline = @headline, Bio = @bio, Phone = @phone, Brokerage = @brokerage,
                LicenseNumber = @license, City = @city, State = @state, PhotoUrl = @photo, WebsiteUrl = @web,
                Languages = @languages, Specialties = @specialties
            WHERE Id = @id
            """, connection);
        BindRealtor(cmd, realtor);
        cmd.ExecuteNonQuery();
    }

    public void TouchLogin(Guid id)
    {
        using var connection = Open();
        using var cmd = new SqlCommand("UPDATE dbo.RealtorNews_Realtors SET LastLoginUtc = SYSUTCDATETIME() WHERE Id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public bool SlugTaken(string slug, Guid? exceptId = null)
    {
        using var connection = Open();
        using var cmd = new SqlCommand("""
            SELECT COUNT(1) FROM dbo.RealtorNews_Realtors
            WHERE Slug = @slug AND (@except IS NULL OR Id <> @except)
            """, connection);
        cmd.Parameters.AddWithValue("@slug", slug);
        cmd.Parameters.AddWithValue("@except", (object?)exceptId ?? DBNull.Value);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public NewsPost SavePost(NewsPost post, bool publish)
    {
        var now = DateTimeOffset.UtcNow;
        post.Category = PostCatalog.NormalizeCategory(post.Category);
        post.PostType = PostCatalog.NormalizeType(post.PostType);
        post.Title = post.Title.Trim();
        post.Body = post.Body.Trim();
        post.Summary = string.IsNullOrWhiteSpace(post.Summary) ? null : post.Summary.Trim();
        post.CoverImageUrl = SanitizeUrl(post.CoverImageUrl);
        post.City = string.IsNullOrWhiteSpace(post.City) ? "Charlotte" : post.City.Trim();
        post.UpdatedUtc = now;
        post.IsPublished = publish;
        if (publish)
        {
            post.PublishedUtc ??= now;
        }

        using var connection = Open();
        if (post.Id == Guid.Empty)
        {
            post.Id = Guid.NewGuid();
            post.Slug = UniquePostSlug(post.Title);
            post.CreatedUtc = now;
            using var insert = new SqlCommand("""
                INSERT INTO dbo.RealtorNews_Posts
                    (Id, RealtorId, PostType, Category, Title, Slug, Summary, Body, CoverImageUrl, City,
                     IsPublished, PublishedUtc, CreatedUtc, UpdatedUtc)
                VALUES
                    (@id, @realtor, @type, @category, @title, @slug, @summary, @body, @image, @city,
                     @published, @publishedUtc, @created, @updated)
                """, connection);
            BindPost(insert, post);
            insert.ExecuteNonQuery();
        }
        else
        {
            using var update = new SqlCommand("""
                UPDATE dbo.RealtorNews_Posts
                SET PostType = @type, Category = @category, Title = @title, Summary = @summary, Body = @body,
                    CoverImageUrl = @image, City = @city, IsPublished = @published, PublishedUtc = @publishedUtc,
                    UpdatedUtc = @updated
                WHERE Id = @id AND RealtorId = @realtor
                """, connection);
            BindPost(update, post);
            update.ExecuteNonQuery();
        }

        return post;
    }

    public NewsPost? GetPost(Guid id, Guid? viewerId = null)
        => QueryPosts(PostSelect + " WHERE p.Id = @id", cmd => cmd.Parameters.AddWithValue("@id", id), viewerId)
            .FirstOrDefault();

    public NewsPost? GetPostBySlug(string slug, Guid? viewerId = null)
        => QueryPosts(PostSelect + " WHERE p.Slug = @slug", cmd => cmd.Parameters.AddWithValue("@slug", slug.Trim().ToLowerInvariant()), viewerId)
            .FirstOrDefault();

    public IReadOnlyList<NewsPost> Published(string? category = null, string? type = null, Guid? viewerId = null, int take = 40)
    {
        var sql = PostSelect + " WHERE p.IsPublished = 1";
        if (!string.IsNullOrWhiteSpace(category) && !category.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            sql += " AND p.Category = @category";
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            sql += " AND p.PostType = @type";
        }

        sql += " ORDER BY COALESCE(p.PublishedUtc, p.CreatedUtc) DESC";
        return QueryPosts(sql, cmd =>
        {
            if (!string.IsNullOrWhiteSpace(category) && !category.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters.AddWithValue("@category", PostCatalog.NormalizeCategory(category));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                cmd.Parameters.AddWithValue("@type", PostCatalog.NormalizeType(type));
            }
        }, viewerId, take);
    }

    public IReadOnlyList<NewsPost> FollowingFeed(Guid viewerId, int take = 40)
        => QueryPosts(PostSelect + """
             WHERE p.IsPublished = 1
               AND (p.RealtorId = @viewer OR EXISTS (
                    SELECT 1 FROM dbo.RealtorNews_Follows f
                    WHERE f.FollowerId = @viewer AND f.FollowingId = p.RealtorId))
             ORDER BY COALESCE(p.PublishedUtc, p.CreatedUtc) DESC
            """, cmd => cmd.Parameters.AddWithValue("@viewer", viewerId), viewerId, take);

    public IReadOnlyList<NewsPost> ForRealtor(Guid realtorId, bool publishedOnly, Guid? viewerId = null)
    {
        var sql = PostSelect + " WHERE p.RealtorId = @realtor";
        if (publishedOnly)
        {
            sql += " AND p.IsPublished = 1";
        }

        sql += " ORDER BY COALESCE(p.PublishedUtc, p.CreatedUtc) DESC";
        return QueryPosts(sql, cmd => cmd.Parameters.AddWithValue("@realtor", realtorId), viewerId);
    }

    public bool DeletePost(Guid id, Guid realtorId)
    {
        using var connection = Open();
        using var cmd = new SqlCommand("DELETE FROM dbo.RealtorNews_Posts WHERE Id = @id AND RealtorId = @realtor", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@realtor", realtorId);
        return cmd.ExecuteNonQuery() > 0;
    }

    public bool ToggleLike(Guid postId, Guid realtorId)
    {
        using var connection = Open();
        using var exists = new SqlCommand("SELECT 1 FROM dbo.RealtorNews_Likes WHERE PostId = @post AND RealtorId = @realtor", connection);
        exists.Parameters.AddWithValue("@post", postId);
        exists.Parameters.AddWithValue("@realtor", realtorId);
        var liked = exists.ExecuteScalar() is not null;
        using var cmd = new SqlCommand(liked
            ? "DELETE FROM dbo.RealtorNews_Likes WHERE PostId = @post AND RealtorId = @realtor"
            : "INSERT INTO dbo.RealtorNews_Likes (PostId, RealtorId) VALUES (@post, @realtor)", connection);
        cmd.Parameters.AddWithValue("@post", postId);
        cmd.Parameters.AddWithValue("@realtor", realtorId);
        cmd.ExecuteNonQuery();
        return !liked;
    }

    public bool ToggleFollow(Guid followerId, Guid followingId)
    {
        if (followerId == followingId)
        {
            return false;
        }

        using var connection = Open();
        using var exists = new SqlCommand("SELECT 1 FROM dbo.RealtorNews_Follows WHERE FollowerId = @a AND FollowingId = @b", connection);
        exists.Parameters.AddWithValue("@a", followerId);
        exists.Parameters.AddWithValue("@b", followingId);
        var following = exists.ExecuteScalar() is not null;
        using var cmd = new SqlCommand(following
            ? "DELETE FROM dbo.RealtorNews_Follows WHERE FollowerId = @a AND FollowingId = @b"
            : "INSERT INTO dbo.RealtorNews_Follows (FollowerId, FollowingId) VALUES (@a, @b)", connection);
        cmd.Parameters.AddWithValue("@a", followerId);
        cmd.Parameters.AddWithValue("@b", followingId);
        cmd.ExecuteNonQuery();
        return !following;
    }

    public IReadOnlyList<PostComment> Comments(Guid postId)
        => QueryComments("""
            SELECT c.Id, c.PostId, c.RealtorId, c.Body, c.CreatedUtc, r.FullName, r.Slug
            FROM dbo.RealtorNews_Comments c
            INNER JOIN dbo.RealtorNews_Realtors r ON r.Id = c.RealtorId
            WHERE c.PostId = @post
            ORDER BY c.CreatedUtc
            """, cmd => cmd.Parameters.AddWithValue("@post", postId));

    public PostComment AddComment(Guid postId, Guid realtorId, string body)
    {
        var comment = new PostComment
        {
            Id = Guid.NewGuid(),
            PostId = postId,
            RealtorId = realtorId,
            Body = body.Trim(),
            CreatedUtc = DateTimeOffset.UtcNow
        };
        using var connection = Open();
        using var cmd = new SqlCommand("""
            INSERT INTO dbo.RealtorNews_Comments (Id, PostId, RealtorId, Body, CreatedUtc)
            VALUES (@id, @post, @realtor, @body, @created)
            """, connection);
        cmd.Parameters.AddWithValue("@id", comment.Id);
        cmd.Parameters.AddWithValue("@post", postId);
        cmd.Parameters.AddWithValue("@realtor", realtorId);
        cmd.Parameters.AddWithValue("@body", comment.Body);
        cmd.Parameters.AddWithValue("@created", comment.CreatedUtc);
        cmd.ExecuteNonQuery();
        return comment;
    }

    public PublicQuestion AddQuestion(PublicQuestion question)
    {
        question.Id = Guid.NewGuid();
        question.CreatedUtc = DateTimeOffset.UtcNow;
        question.Status = "open";
        using var connection = Open();
        using var cmd = new SqlCommand("""
            INSERT INTO dbo.RealtorNews_Questions (Id, FirstName, Email, Topic, Body, CreatedUtc, Status)
            VALUES (@id, @name, @email, @topic, @body, @created, @status)
            """, connection);
        cmd.Parameters.AddWithValue("@id", question.Id);
        cmd.Parameters.AddWithValue("@name", question.FirstName.Trim());
        cmd.Parameters.AddWithValue("@email", question.Email.Trim().ToLowerInvariant());
        cmd.Parameters.AddWithValue("@topic", (object?)question.Topic ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@body", question.Body.Trim());
        cmd.Parameters.AddWithValue("@created", question.CreatedUtc);
        cmd.Parameters.AddWithValue("@status", question.Status);
        cmd.ExecuteNonQuery();
        return question;
    }

    public IReadOnlyList<PublicQuestion> OpenQuestions(int take = 20)
    {
        var items = new List<PublicQuestion>();
        using var connection = Open();
        using var cmd = new SqlCommand("""
            SELECT TOP (@take) Id, FirstName, Email, Topic, Body, CreatedUtc, Status, AnswerPostId
            FROM dbo.RealtorNews_Questions
            WHERE Status = N'open'
            ORDER BY CreatedUtc DESC
            """, connection);
        cmd.Parameters.AddWithValue("@take", take);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            items.Add(ReadQuestion(reader));
        }

        return items;
    }

    public int PublishedCount()
    {
        using var connection = Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.RealtorNews_Posts WHERE IsPublished = 1", connection);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public int RealtorCount()
    {
        using var connection = Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.RealtorNews_Realtors WHERE IsActive = 1", connection);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public static string? SanitizeUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
        {
            return null;
        }

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            return null;
        }

        return uri.ToString();
    }

    private const string RealtorSelect = """
        SELECT r.*,
            (SELECT COUNT(*) FROM dbo.RealtorNews_Posts p WHERE p.RealtorId = r.Id AND p.IsPublished = 1) AS PostCount,
            (SELECT COUNT(*) FROM dbo.RealtorNews_Follows f WHERE f.FollowingId = r.Id) AS FollowerCount,
            (SELECT COUNT(*) FROM dbo.RealtorNews_Follows f WHERE f.FollowerId = r.Id) AS FollowingCount,
            CAST(CASE WHEN @viewer IS NOT NULL AND EXISTS (
                SELECT 1 FROM dbo.RealtorNews_Follows x WHERE x.FollowerId = @viewer AND x.FollowingId = r.Id
            ) THEN 1 ELSE 0 END AS bit) AS FollowedByMe
        FROM dbo.RealtorNews_Realtors r
        """;

    private const string PostSelect = """
        SELECT p.Id, p.RealtorId, p.PostType, p.Category, p.Title, p.Slug, p.Summary, p.Body, p.CoverImageUrl,
               p.City, p.IsPublished, p.PublishedUtc, p.CreatedUtc, p.UpdatedUtc,
               r.FullName, r.Slug AS RealtorSlug, r.Brokerage, r.PhotoUrl, r.Headline,
               (SELECT COUNT(*) FROM dbo.RealtorNews_Likes l WHERE l.PostId = p.Id) AS LikeCount,
               (SELECT COUNT(*) FROM dbo.RealtorNews_Comments c WHERE c.PostId = p.Id) AS CommentCount,
               CAST(CASE WHEN @viewer IS NOT NULL AND EXISTS (
                    SELECT 1 FROM dbo.RealtorNews_Likes l WHERE l.PostId = p.Id AND l.RealtorId = @viewer
               ) THEN 1 ELSE 0 END AS bit) AS LikedByMe,
               CAST(CASE WHEN @viewer IS NOT NULL AND EXISTS (
                    SELECT 1 FROM dbo.RealtorNews_Follows f WHERE f.FollowerId = @viewer AND f.FollowingId = p.RealtorId
               ) THEN 1 ELSE 0 END AS bit) AS FollowingAuthor
        FROM dbo.RealtorNews_Posts p
        INNER JOIN dbo.RealtorNews_Realtors r ON r.Id = p.RealtorId
        """;

    private string UniqueRealtorSlug(string name)
    {
        var root = SlugMaker.From(name);
        var slug = root;
        var n = 2;
        while (SlugTaken(slug))
        {
            slug = $"{root}-{n++}";
        }

        return slug;
    }

    private string UniquePostSlug(string title)
    {
        var root = SlugMaker.From(title, 180);
        var slug = root;
        var n = 2;
        while (PostSlugTaken(slug))
        {
            slug = $"{root}-{n++}";
        }

        return slug;
    }

    private bool PostSlugTaken(string slug)
    {
        using var connection = Open();
        using var cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.RealtorNews_Posts WHERE Slug = @slug", connection);
        cmd.Parameters.AddWithValue("@slug", slug);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    private List<RealtorAccount> QueryRealtors(string sql, Action<SqlCommand>? bind, Guid? viewerId = null)
    {
        var list = new List<RealtorAccount>();
        using var connection = Open();
        using var cmd = new SqlCommand(sql, connection);
        if (sql.Contains("@viewer", StringComparison.Ordinal))
        {
            cmd.Parameters.AddWithValue("@viewer", (object?)viewerId ?? DBNull.Value);
        }

        bind?.Invoke(cmd);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(ReadRealtor(reader));
        }

        return list;
    }

    private List<NewsPost> QueryPosts(string sql, Action<SqlCommand>? bind, Guid? viewerId, int take = 200)
    {
        var list = new List<NewsPost>();
        using var connection = Open();
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@viewer", (object?)viewerId ?? DBNull.Value);
        bind?.Invoke(cmd);
        using var reader = cmd.ExecuteReader();
        while (reader.Read() && list.Count < take)
        {
            list.Add(ReadPost(reader));
        }

        return list;
    }

    private List<PostComment> QueryComments(string sql, Action<SqlCommand> bind)
    {
        var list = new List<PostComment>();
        using var connection = Open();
        using var cmd = new SqlCommand(sql, connection);
        bind(cmd);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new PostComment
            {
                Id = reader.GetGuid(0),
                PostId = reader.GetGuid(1),
                RealtorId = reader.GetGuid(2),
                Body = reader.GetString(3),
                CreatedUtc = reader.GetFieldValue<DateTimeOffset>(4),
                RealtorName = reader.GetString(5),
                RealtorSlug = reader.GetString(6)
            });
        }

        return list;
    }

    private static RealtorAccount ReadRealtor(SqlDataReader reader)
        => new()
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
            FullName = reader.GetString(reader.GetOrdinal("FullName")),
            Slug = reader.GetString(reader.GetOrdinal("Slug")),
            Headline = reader.IsDBNull(reader.GetOrdinal("Headline")) ? null : reader.GetString(reader.GetOrdinal("Headline")),
            Bio = reader.IsDBNull(reader.GetOrdinal("Bio")) ? null : reader.GetString(reader.GetOrdinal("Bio")),
            Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
            Brokerage = reader.IsDBNull(reader.GetOrdinal("Brokerage")) ? null : reader.GetString(reader.GetOrdinal("Brokerage")),
            LicenseNumber = reader.IsDBNull(reader.GetOrdinal("LicenseNumber")) ? null : reader.GetString(reader.GetOrdinal("LicenseNumber")),
            City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
            State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
            PhotoUrl = reader.IsDBNull(reader.GetOrdinal("PhotoUrl")) ? null : reader.GetString(reader.GetOrdinal("PhotoUrl")),
            WebsiteUrl = reader.IsDBNull(reader.GetOrdinal("WebsiteUrl")) ? null : reader.GetString(reader.GetOrdinal("WebsiteUrl")),
            Languages = reader.IsDBNull(reader.GetOrdinal("Languages")) ? null : reader.GetString(reader.GetOrdinal("Languages")),
            Specialties = reader.IsDBNull(reader.GetOrdinal("Specialties")) ? null : reader.GetString(reader.GetOrdinal("Specialties")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
            CreatedUtc = reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal("CreatedUtc")),
            LastLoginUtc = reader.IsDBNull(reader.GetOrdinal("LastLoginUtc")) ? null : reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal("LastLoginUtc")),
            PostCount = reader.GetInt32(reader.GetOrdinal("PostCount")),
            FollowerCount = reader.GetInt32(reader.GetOrdinal("FollowerCount")),
            FollowingCount = reader.GetInt32(reader.GetOrdinal("FollowingCount")),
            FollowedByMe = reader.GetBoolean(reader.GetOrdinal("FollowedByMe"))
        };

    private static NewsPost ReadPost(SqlDataReader reader)
        => new()
        {
            Id = reader.GetGuid(0),
            RealtorId = reader.GetGuid(1),
            PostType = reader.GetString(2),
            Category = reader.GetString(3),
            Title = reader.GetString(4),
            Slug = reader.GetString(5),
            Summary = reader.IsDBNull(6) ? null : reader.GetString(6),
            Body = reader.IsDBNull(7) ? "" : reader.GetString(7),
            CoverImageUrl = reader.IsDBNull(8) ? null : reader.GetString(8),
            City = reader.IsDBNull(9) ? null : reader.GetString(9),
            IsPublished = reader.GetBoolean(10),
            PublishedUtc = reader.IsDBNull(11) ? null : reader.GetFieldValue<DateTimeOffset>(11),
            CreatedUtc = reader.GetFieldValue<DateTimeOffset>(12),
            UpdatedUtc = reader.GetFieldValue<DateTimeOffset>(13),
            RealtorName = reader.GetString(14),
            RealtorSlug = reader.GetString(15),
            RealtorBrokerage = reader.IsDBNull(16) ? null : reader.GetString(16),
            RealtorPhotoUrl = reader.IsDBNull(17) ? null : reader.GetString(17),
            RealtorHeadline = reader.IsDBNull(18) ? null : reader.GetString(18),
            LikeCount = reader.GetInt32(19),
            CommentCount = reader.GetInt32(20),
            LikedByMe = reader.GetBoolean(21),
            FollowingAuthor = reader.GetBoolean(22)
        };

    private static PublicQuestion ReadQuestion(SqlDataReader reader)
        => new()
        {
            Id = reader.GetGuid(0),
            FirstName = reader.GetString(1),
            Email = reader.GetString(2),
            Topic = reader.IsDBNull(3) ? null : reader.GetString(3),
            Body = reader.GetString(4),
            CreatedUtc = reader.GetFieldValue<DateTimeOffset>(5),
            Status = reader.GetString(6),
            AnswerPostId = reader.IsDBNull(7) ? null : reader.GetGuid(7)
        };

    private static void BindRealtor(SqlCommand cmd, RealtorAccount realtor)
    {
        cmd.Parameters.AddWithValue("@id", realtor.Id);
        cmd.Parameters.AddWithValue("@email", realtor.Email);
        cmd.Parameters.AddWithValue("@hash", realtor.PasswordHash);
        cmd.Parameters.AddWithValue("@name", realtor.FullName.Trim());
        cmd.Parameters.AddWithValue("@slug", realtor.Slug);
        cmd.Parameters.AddWithValue("@headline", (object?)realtor.Headline ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@bio", (object?)realtor.Bio ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@phone", (object?)realtor.Phone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@brokerage", (object?)realtor.Brokerage ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@license", (object?)realtor.LicenseNumber ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@city", (object?)realtor.City ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@state", (object?)realtor.State ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@photo", (object?)SanitizeUrl(realtor.PhotoUrl) ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@web", (object?)SanitizeUrl(realtor.WebsiteUrl) ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@languages", (object?)realtor.Languages ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@specialties", (object?)realtor.Specialties ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@created", realtor.CreatedUtc);
    }

    private static void BindPost(SqlCommand cmd, NewsPost post)
    {
        cmd.Parameters.AddWithValue("@id", post.Id);
        cmd.Parameters.AddWithValue("@realtor", post.RealtorId);
        cmd.Parameters.AddWithValue("@type", post.PostType);
        cmd.Parameters.AddWithValue("@category", post.Category);
        cmd.Parameters.AddWithValue("@title", post.Title);
        cmd.Parameters.AddWithValue("@slug", post.Slug);
        cmd.Parameters.AddWithValue("@summary", (object?)post.Summary ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@body", post.Body);
        cmd.Parameters.AddWithValue("@image", (object?)post.CoverImageUrl ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@city", (object?)post.City ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@published", post.IsPublished);
        cmd.Parameters.AddWithValue("@publishedUtc", (object?)post.PublishedUtc ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@created", post.CreatedUtc);
        cmd.Parameters.AddWithValue("@updated", post.UpdatedUtc);
    }

    private SqlConnection Open()
    {
        if (!IsConfigured)
        {
            throw new InvalidOperationException("Falta ConnectionStrings:IndorDB.");
        }

        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
