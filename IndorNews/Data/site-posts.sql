-- IndorDB — noticias diarias (SAFE / roofing / realtor / carolina / realestatedesk / safecomfort)
IF OBJECT_ID(N'dbo.SitePosts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SitePosts
    (
        Id          uniqueidentifier  NOT NULL
            CONSTRAINT PK_SitePosts PRIMARY KEY,
        SiteKey     nvarchar(32)      NOT NULL,
        Title       nvarchar(180)     NOT NULL,
        Body        nvarchar(4000)    NOT NULL
            CONSTRAINT DF_SitePosts_Body DEFAULT (N''),
        ImageUrl    nvarchar(2048)    NULL,
        CreatedUtc  datetimeoffset(7) NOT NULL
            CONSTRAINT DF_SitePosts_CreatedUtc DEFAULT (SYSUTCDATETIME())
    );

    CREATE INDEX IX_SitePosts_SiteKey_CreatedUtc
        ON dbo.SitePosts (SiteKey, CreatedUtc DESC);
END
GO

-- Lista de sitios permitidos. Se vuelve a crear para que al agregar un sitio baste con correr este script.
IF OBJECT_ID(N'dbo.CK_SitePosts_SiteKey', N'C') IS NOT NULL
    ALTER TABLE dbo.SitePosts DROP CONSTRAINT CK_SitePosts_SiteKey;
GO

ALTER TABLE dbo.SitePosts ADD CONSTRAINT CK_SitePosts_SiteKey CHECK
    (SiteKey IN (N'safe', N'roofing', N'realtor', N'carolina', N'realestatedesk', N'safecomfort'));
GO
