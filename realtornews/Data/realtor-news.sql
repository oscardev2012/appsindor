-- IndorDB — tablas NUEVAS para Charlotte Realtor Recommendations
-- No modifica tablas existentes (SitePosts u otras).
-- Ejecuta este script en SQL Server / Azure SQL (IndorDB) y luego arranca la app.

IF OBJECT_ID(N'dbo.RealtorNews_Realtors', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RealtorNews_Realtors
    (
        Id             uniqueidentifier  NOT NULL
            CONSTRAINT PK_RealtorNews_Realtors PRIMARY KEY,
        Email          nvarchar(256)     NOT NULL,
        PasswordHash   nvarchar(500)     NOT NULL,
        FullName       nvarchar(120)     NOT NULL,
        Slug           nvarchar(140)     NOT NULL,
        Headline       nvarchar(180)     NULL,
        Bio            nvarchar(4000)    NULL,
        Phone          nvarchar(40)      NULL,
        Brokerage      nvarchar(160)     NULL,
        LicenseNumber  nvarchar(80)      NULL,
        City           nvarchar(80)      NULL,
        State          nvarchar(40)      NULL,
        PhotoUrl       nvarchar(2048)    NULL,
        WebsiteUrl     nvarchar(2048)    NULL,
        Languages      nvarchar(120)     NULL,
        Specialties    nvarchar(240)     NULL,
        IsActive       bit               NOT NULL
            CONSTRAINT DF_RealtorNews_Realtors_IsActive DEFAULT (1),
        CreatedUtc     datetimeoffset(7) NOT NULL
            CONSTRAINT DF_RealtorNews_Realtors_CreatedUtc DEFAULT (SYSUTCDATETIME()),
        LastLoginUtc   datetimeoffset(7) NULL,
        CONSTRAINT UQ_RealtorNews_Realtors_Email UNIQUE (Email),
        CONSTRAINT UQ_RealtorNews_Realtors_Slug UNIQUE (Slug)
    );
END
GO

IF OBJECT_ID(N'dbo.RealtorNews_Posts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RealtorNews_Posts
    (
        Id             uniqueidentifier  NOT NULL
            CONSTRAINT PK_RealtorNews_Posts PRIMARY KEY,
        RealtorId      uniqueidentifier  NOT NULL,
        PostType       nvarchar(20)      NOT NULL,
        Category       nvarchar(40)      NOT NULL,
        Title          nvarchar(180)     NOT NULL,
        Slug           nvarchar(200)     NOT NULL,
        Summary        nvarchar(500)     NULL,
        Body           nvarchar(max)     NOT NULL
            CONSTRAINT DF_RealtorNews_Posts_Body DEFAULT (N''),
        CoverImageUrl  nvarchar(2048)    NULL,
        City           nvarchar(80)      NULL,
        IsPublished    bit               NOT NULL
            CONSTRAINT DF_RealtorNews_Posts_IsPublished DEFAULT (1),
        PublishedUtc   datetimeoffset(7) NULL,
        CreatedUtc     datetimeoffset(7) NOT NULL
            CONSTRAINT DF_RealtorNews_Posts_CreatedUtc DEFAULT (SYSUTCDATETIME()),
        UpdatedUtc     datetimeoffset(7) NOT NULL
            CONSTRAINT DF_RealtorNews_Posts_UpdatedUtc DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT FK_RealtorNews_Posts_Realtor
            FOREIGN KEY (RealtorId) REFERENCES dbo.RealtorNews_Realtors (Id),
        CONSTRAINT UQ_RealtorNews_Posts_Slug UNIQUE (Slug),
        CONSTRAINT CK_RealtorNews_Posts_PostType
            CHECK (PostType IN (N'feed', N'article'))
    );

    CREATE INDEX IX_RealtorNews_Posts_Published
        ON dbo.RealtorNews_Posts (IsPublished, PublishedUtc DESC);

    CREATE INDEX IX_RealtorNews_Posts_Realtor
        ON dbo.RealtorNews_Posts (RealtorId, PublishedUtc DESC);

    CREATE INDEX IX_RealtorNews_Posts_Category
        ON dbo.RealtorNews_Posts (Category, PublishedUtc DESC);
END
GO

IF OBJECT_ID(N'dbo.RealtorNews_Comments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RealtorNews_Comments
    (
        Id          uniqueidentifier  NOT NULL
            CONSTRAINT PK_RealtorNews_Comments PRIMARY KEY,
        PostId      uniqueidentifier  NOT NULL,
        RealtorId   uniqueidentifier  NOT NULL,
        Body        nvarchar(2000)    NOT NULL,
        CreatedUtc  datetimeoffset(7) NOT NULL
            CONSTRAINT DF_RealtorNews_Comments_CreatedUtc DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT FK_RealtorNews_Comments_Post
            FOREIGN KEY (PostId) REFERENCES dbo.RealtorNews_Posts (Id) ON DELETE CASCADE,
        CONSTRAINT FK_RealtorNews_Comments_Realtor
            FOREIGN KEY (RealtorId) REFERENCES dbo.RealtorNews_Realtors (Id)
    );

    CREATE INDEX IX_RealtorNews_Comments_Post
        ON dbo.RealtorNews_Comments (PostId, CreatedUtc);
END
GO

IF OBJECT_ID(N'dbo.RealtorNews_Likes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RealtorNews_Likes
    (
        PostId      uniqueidentifier  NOT NULL,
        RealtorId   uniqueidentifier  NOT NULL,
        CreatedUtc  datetimeoffset(7) NOT NULL
            CONSTRAINT DF_RealtorNews_Likes_CreatedUtc DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_RealtorNews_Likes PRIMARY KEY (PostId, RealtorId),
        CONSTRAINT FK_RealtorNews_Likes_Post
            FOREIGN KEY (PostId) REFERENCES dbo.RealtorNews_Posts (Id) ON DELETE CASCADE,
        CONSTRAINT FK_RealtorNews_Likes_Realtor
            FOREIGN KEY (RealtorId) REFERENCES dbo.RealtorNews_Realtors (Id)
    );
END
GO

IF OBJECT_ID(N'dbo.RealtorNews_Follows', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RealtorNews_Follows
    (
        FollowerId   uniqueidentifier  NOT NULL,
        FollowingId  uniqueidentifier  NOT NULL,
        CreatedUtc   datetimeoffset(7) NOT NULL
            CONSTRAINT DF_RealtorNews_Follows_CreatedUtc DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_RealtorNews_Follows PRIMARY KEY (FollowerId, FollowingId),
        CONSTRAINT FK_RealtorNews_Follows_Follower
            FOREIGN KEY (FollowerId) REFERENCES dbo.RealtorNews_Realtors (Id),
        CONSTRAINT FK_RealtorNews_Follows_Following
            FOREIGN KEY (FollowingId) REFERENCES dbo.RealtorNews_Realtors (Id),
        CONSTRAINT CK_RealtorNews_Follows_NotSelf CHECK (FollowerId <> FollowingId)
    );

    CREATE INDEX IX_RealtorNews_Follows_Following
        ON dbo.RealtorNews_Follows (FollowingId);
END
GO

IF OBJECT_ID(N'dbo.RealtorNews_Questions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RealtorNews_Questions
    (
        Id            uniqueidentifier  NOT NULL
            CONSTRAINT PK_RealtorNews_Questions PRIMARY KEY,
        FirstName     nvarchar(80)      NOT NULL,
        Email         nvarchar(256)     NOT NULL,
        Topic         nvarchar(80)      NULL,
        Body          nvarchar(2000)    NOT NULL,
        CreatedUtc    datetimeoffset(7) NOT NULL
            CONSTRAINT DF_RealtorNews_Questions_CreatedUtc DEFAULT (SYSUTCDATETIME()),
        Status        nvarchar(20)      NOT NULL
            CONSTRAINT DF_RealtorNews_Questions_Status DEFAULT (N'open'),
        AnswerPostId  uniqueidentifier  NULL,
        CONSTRAINT CK_RealtorNews_Questions_Status
            CHECK (Status IN (N'open', N'answered')),
        CONSTRAINT FK_RealtorNews_Questions_Answer
            FOREIGN KEY (AnswerPostId) REFERENCES dbo.RealtorNews_Posts (Id)
    );

    CREATE INDEX IX_RealtorNews_Questions_Status
        ON dbo.RealtorNews_Questions (Status, CreatedUtc DESC);
END
GO
