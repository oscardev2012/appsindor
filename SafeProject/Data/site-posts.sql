-- IndorDB — daily homepage posts (SAFE / roofing / realtor / carolina)
-- Run this in SSMS or Azure Query editor against IndorDB.

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
            CONSTRAINT DF_SitePosts_CreatedUtc DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT CK_SitePosts_SiteKey CHECK
            (SiteKey IN (N'safe', N'roofing', N'realtor', N'carolina'))
    );

    CREATE INDEX IX_SitePosts_SiteKey_CreatedUtc
        ON dbo.SitePosts (SiteKey, CreatedUtc DESC);
END
GO
