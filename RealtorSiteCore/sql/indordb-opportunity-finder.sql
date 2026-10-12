-- NC Opportunity Finder: tablas en IndorDB.
-- Correr conectado a la base IndorDB (local o Azure SQL); no usa USE porque Azure SQL no lo permite.
-- Se puede correr más de una vez; solo crea lo que falta.
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'dbo.NcOppProperties', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NcOppProperties
    (
        Id                INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_NcOppProperties PRIMARY KEY,
        PublicId          AS (N'PROP-' + RIGHT(N'00000' + CAST(Id AS NVARCHAR(10)), 5)) PERSISTED,
        ExternalId        NVARCHAR(200)  NULL,
        Source            NVARCHAR(60)   NOT NULL CONSTRAINT DF_NcOppProperties_Source DEFAULT (N'Facebook'),
        SourceUrl         NVARCHAR(1000) NULL,
        PostType          NVARCHAR(120)  NULL,
        PostedAt          DATETIME2      NULL,
        ImportedAt        DATETIME2      NOT NULL CONSTRAINT DF_NcOppProperties_ImportedAt DEFAULT (SYSUTCDATETIME()),
        UpdatedAt         DATETIME2      NOT NULL CONSTRAINT DF_NcOppProperties_UpdatedAt DEFAULT (SYSUTCDATETIME()),
        Title             NVARCHAR(300)  NOT NULL,
        Address           NVARCHAR(300)  NULL,
        City              NVARCHAR(120)  NULL,
        State             NVARCHAR(2)    NOT NULL CONSTRAINT DF_NcOppProperties_State DEFAULT (N'NC'),
        Zip               NVARCHAR(10)   NULL,
        Beds              DECIMAL(4,1)   NULL,
        Baths             DECIMAL(4,1)   NULL,
        AskingPrice       DECIMAL(12,2)  NULL,
        SellerSignal      NVARCHAR(400)  NULL,
        RawText           NVARCHAR(MAX)  NULL,
        Images            NVARCHAR(MAX)  NOT NULL CONSTRAINT DF_NcOppProperties_Images DEFAULT (N'[]'),
        ComparablesCount  INT            NULL,
        ArvLow            DECIMAL(12,2)  NULL,
        ArvHigh           DECIMAL(12,2)  NULL,
        ArvEstimate       DECIMAL(12,2)  NULL,
        RepairsLow        DECIMAL(12,2)  NULL,
        RepairsHigh       DECIMAL(12,2)  NULL,
        RepairsEstimate   DECIMAL(12,2)  NULL,
        ClosingHolding    DECIMAL(12,2)  NULL,
        Contingency       DECIMAL(12,2)  NULL,
        Score             INT            NULL CONSTRAINT CK_NcOppProperties_Score CHECK (Score BETWEEN 0 AND 100),
        Notes             NVARCHAR(1000) NULL,
        Status            NVARCHAR(20)   NOT NULL CONSTRAINT DF_NcOppProperties_Status DEFAULT (N'Active')
                          CONSTRAINT CK_NcOppProperties_Status CHECK (Status IN (N'Active', N'HumanReview', N'Discarded')),
        ReviewRequestedAt DATETIME2      NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_NcOppProperties_ExternalId')
    CREATE UNIQUE INDEX UX_NcOppProperties_ExternalId ON dbo.NcOppProperties (ExternalId) WHERE ExternalId IS NOT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_NcOppProperties_Status_ImportedAt')
    CREATE INDEX IX_NcOppProperties_Status_ImportedAt ON dbo.NcOppProperties (Status, ImportedAt DESC);
GO

IF OBJECT_ID(N'dbo.NcOppPropertyEvents', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NcOppPropertyEvents
    (
        Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_NcOppPropertyEvents PRIMARY KEY,
        PropertyId INT           NOT NULL CONSTRAINT FK_NcOppPropertyEvents_Property
                   REFERENCES dbo.NcOppProperties (Id) ON DELETE CASCADE,
        Kind       NVARCHAR(40)  NOT NULL,
        Detail     NVARCHAR(MAX) NOT NULL CONSTRAINT DF_NcOppPropertyEvents_Detail DEFAULT (N'{}'),
        CreatedAt  DATETIME2     NOT NULL CONSTRAINT DF_NcOppPropertyEvents_CreatedAt DEFAULT (SYSUTCDATETIME())
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_NcOppPropertyEvents_PropertyId')
    CREATE INDEX IX_NcOppPropertyEvents_PropertyId ON dbo.NcOppPropertyEvents (PropertyId, CreatedAt DESC);
GO

IF COL_LENGTH(N'dbo.NcOppProperties', N'SellerName') IS NULL
    ALTER TABLE dbo.NcOppProperties ADD SellerName NVARCHAR(200) NULL;
GO

IF COL_LENGTH(N'dbo.NcOppProperties', N'SellerPhone') IS NULL
    ALTER TABLE dbo.NcOppProperties ADD SellerPhone NVARCHAR(100) NULL;
GO

IF COL_LENGTH(N'dbo.NcOppProperties', N'SellerEmail') IS NULL
    ALTER TABLE dbo.NcOppProperties ADD SellerEmail NVARCHAR(300) NULL;
GO

IF COL_LENGTH(N'dbo.NcOppProperties', N'SellerProfileUrl') IS NULL
    ALTER TABLE dbo.NcOppProperties ADD SellerProfileUrl NVARCHAR(500) NULL;
GO

IF COL_LENGTH(N'dbo.NcOppProperties', N'MarketValue') IS NULL
    ALTER TABLE dbo.NcOppProperties ADD MarketValue DECIMAL(12,2) NULL;
GO

IF COL_LENGTH(N'dbo.NcOppProperties', N'Sqft') IS NULL
    ALTER TABLE dbo.NcOppProperties ADD Sqft INT NULL;
GO

IF COL_LENGTH(N'dbo.NcOppProperties', N'Latitude') IS NULL
    ALTER TABLE dbo.NcOppProperties ADD Latitude DECIMAL(9,6) NULL, Longitude DECIMAL(9,6) NULL;
GO

IF COL_LENGTH(N'dbo.NcOppProperties', N'Sources') IS NULL
    ALTER TABLE dbo.NcOppProperties ADD Sources NVARCHAR(400) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_NcOppProperties_State_City')
    CREATE INDEX IX_NcOppProperties_State_City ON dbo.NcOppProperties (State, City);
GO

-- Cada búsqueda en Apify: ciudad, fuentes elegidas y el resultado de cada fuente.
IF OBJECT_ID(N'dbo.NcOppSearches', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NcOppSearches
    (
        Id            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_NcOppSearches PRIMARY KEY,
        StartedAt     DATETIME2      NOT NULL CONSTRAINT DF_NcOppSearches_StartedAt DEFAULT (SYSUTCDATETIME()),
        FinishedAt    DATETIME2      NULL,
        Status        NVARCHAR(20)   NOT NULL CONSTRAINT DF_NcOppSearches_Status DEFAULT (N'Running')
                      CONSTRAINT CK_NcOppSearches_Status CHECK (Status IN (N'Running', N'Succeeded', N'Partial', N'Failed')),
        State         NVARCHAR(2)    NOT NULL,
        City          NVARCHAR(120)  NOT NULL,
        Options       NVARCHAR(MAX)  NOT NULL,
        SourceResults NVARCHAR(MAX)  NOT NULL CONSTRAINT DF_NcOppSearches_SourceResults DEFAULT (N'{}'),
        Found         INT            NOT NULL CONSTRAINT DF_NcOppSearches_Found DEFAULT (0),
        Imported      INT            NOT NULL CONSTRAINT DF_NcOppSearches_Imported DEFAULT (0),
        Updated       INT            NOT NULL CONSTRAINT DF_NcOppSearches_Updated DEFAULT (0),
        Skipped       INT            NOT NULL CONSTRAINT DF_NcOppSearches_Skipped DEFAULT (0),
        Error         NVARCHAR(1000) NULL
    );
END
GO

SELECT name FROM sys.tables WHERE name LIKE N'NcOpp%' ORDER BY name;
GO
