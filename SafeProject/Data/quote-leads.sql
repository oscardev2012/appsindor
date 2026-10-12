-- IndorDB — quote / lead registrations from /water-heater-replacement-quote/
-- Run in SSMS or Azure Query editor against database IndorDB.

IF OBJECT_ID(N'dbo.QuoteLeads', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QuoteLeads
    (
        Id            uniqueidentifier  NOT NULL
            CONSTRAINT PK_QuoteLeads PRIMARY KEY,
        Reference     nvarchar(32)      NOT NULL,
        FullName      nvarchar(120)     NOT NULL,
        Phone         nvarchar(40)      NOT NULL,
        Email         nvarchar(180)     NULL,
        Address       nvarchar(240)     NOT NULL,
        City          nvarchar(80)      NOT NULL,
        Fuel          nvarchar(40)      NOT NULL,
        TankSize      nvarchar(40)      NULL,
        Issue         nvarchar(500)     NULL,
        SmsConsent    bit               NOT NULL
            CONSTRAINT DF_QuoteLeads_SmsConsent DEFAULT (0),
        UtmSource     nvarchar(80)      NULL,
        UtmCampaign   nvarchar(120)     NULL,
        Gclid         nvarchar(120)     NULL,
        LandingPath   nvarchar(200)     NULL,
        CreatedUtc    datetimeoffset(7) NOT NULL
            CONSTRAINT DF_QuoteLeads_CreatedUtc DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_QuoteLeads_Reference UNIQUE (Reference)
    );

    CREATE INDEX IX_QuoteLeads_CreatedUtc
        ON dbo.QuoteLeads (CreatedUtc DESC);

    CREATE INDEX IX_QuoteLeads_Phone
        ON dbo.QuoteLeads (Phone);
END
GO
