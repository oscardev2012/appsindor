/* SAFE Ecosystem + Core + Operations  —  run this in IndorDB (SQL Server)
   All three portals read and write these same tables.
   Order: run the whole script once. Safe to re-run (IF NOT EXISTS). */

USE IndorDB;
GO

IF OBJECT_ID(N'dbo.SafeContacts', N'U') IS NULL
CREATE TABLE dbo.SafeContacts (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  Role          NVARCHAR(30)  NOT NULL, -- Realtor | Worker | Client | Operations
  Name          NVARCHAR(160) NOT NULL,
  Email         NVARCHAR(160) NULL,
  Phone         NVARCHAR(40)  NULL,
  Firm          NVARCHAR(160) NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeContacts_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeContacts_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeProperties', N'U') IS NULL
CREATE TABLE dbo.SafeProperties (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  ClientId      INT           NULL,
  RealtorId     INT           NULL,
  Address       NVARCHAR(200) NOT NULL,
  City          NVARCHAR(120) NULL,
  Status        NVARCHAR(40)  NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeProperties_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeProperties_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeRequests', N'U') IS NULL
CREATE TABLE dbo.SafeRequests (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  RealtorId     INT           NULL,
  ClientId      INT           NULL,
  PropertyId    INT           NULL,
  SourceApp     NVARCHAR(30)  NOT NULL, -- Ecosystem | Core | Operations
  Source        NVARCHAR(80)  NULL,
  Type          NVARCHAR(120) NULL,
  Status        NVARCHAR(60)  NOT NULL,
  Priority      NVARCHAR(20)  NULL,
  Owner         NVARCHAR(80)  NULL,
  ContactName   NVARCHAR(160) NULL,
  ClientName    NVARCHAR(160) NULL,
  Address       NVARCHAR(200) NOT NULL,
  City          NVARCHAR(120) NULL,
  Notes         NVARCHAR(MAX) NULL,
  Items         INT           NOT NULL CONSTRAINT DF_SafeRequests_Items DEFAULT 0,
  Value         DECIMAL(18,2) NOT NULL CONSTRAINT DF_SafeRequests_Value DEFAULT 0,
  DueAt         DATETIME2     NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeRequests_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeRequests_UpdatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeRequests_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeEstimates', N'U') IS NULL
CREATE TABLE dbo.SafeEstimates (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  RequestId     INT           NOT NULL,
  Status        NVARCHAR(40)  NOT NULL,
  Total         DECIMAL(18,2) NOT NULL CONSTRAINT DF_SafeEstimates_Total DEFAULT 0,
  RangeText     NVARCHAR(80)  NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeEstimates_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeEstimates_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeWorkOrders', N'U') IS NULL
CREATE TABLE dbo.SafeWorkOrders (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  RequestId     INT           NOT NULL,
  EstimateId    INT           NULL,
  WorkerId      INT           NULL,
  Status        NVARCHAR(40)  NOT NULL,
  Progress      INT           NOT NULL CONSTRAINT DF_SafeWorkOrders_Progress DEFAULT 0,
  Scope         NVARCHAR(200) NULL,
  CheckInAt     DATETIME2     NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeWorkOrders_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeWorkOrders_UpdatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeWorkOrders_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeThreads', N'U') IS NULL
CREATE TABLE dbo.SafeThreads (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  RequestId     INT           NOT NULL,
  WorkOrderId   INT           NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeThreads_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeThreads_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeMessages', N'U') IS NULL
CREATE TABLE dbo.SafeMessages (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  ThreadId      INT           NOT NULL,
  SenderRole    NVARCHAR(30)  NOT NULL, -- Realtor | Worker | Operations | Client
  SenderName    NVARCHAR(160) NOT NULL,
  Channel       NVARCHAR(40)  NOT NULL, -- job | email | sms | promo
  Body          NVARCHAR(MAX) NOT NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeMessages_CreatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafePromotions', N'U') IS NULL
CREATE TABLE dbo.SafePromotions (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  Title         NVARCHAR(160) NOT NULL,
  Offer         NVARCHAR(200) NULL,
  Body          NVARCHAR(MAX) NULL,
  Status        NVARCHAR(30)  NOT NULL, -- Draft | Live | Retired
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafePromotions_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafePromotions_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeCampaigns', N'U') IS NULL
CREATE TABLE dbo.SafeCampaigns (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  RealtorId     INT           NULL,
  Title         NVARCHAR(160) NOT NULL,
  Body          NVARCHAR(MAX) NULL,
  Status        NVARCHAR(30)  NOT NULL, -- Draft | Sent | Scheduled
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeCampaigns_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeCampaigns_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeLeads', N'U') IS NULL
CREATE TABLE dbo.SafeLeads (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  Name          NVARCHAR(160) NOT NULL,
  Channel       NVARCHAR(80)  NULL,
  Stage         NVARCHAR(40)  NOT NULL, -- New | Qualified | Conversation | Demo
  Notes         NVARCHAR(400) NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeLeads_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeLeads_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeInvoices', N'U') IS NULL
CREATE TABLE dbo.SafeInvoices (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  RequestId     INT           NULL,
  PropertyId    INT           NULL,
  Total         DECIMAL(18,2) NOT NULL CONSTRAINT DF_SafeInvoices_Total DEFAULT 0,
  Status        NVARCHAR(40)  NOT NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeInvoices_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeInvoices_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeGiftCards', N'U') IS NULL
CREATE TABLE dbo.SafeGiftCards (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  PublicId      NVARCHAR(40)  NOT NULL,
  ClientName    NVARCHAR(160) NULL,
  Address       NVARCHAR(200) NULL,
  Firm          NVARCHAR(160) NULL,
  Amount        DECIMAL(18,2) NOT NULL,
  Code          NVARCHAR(40)  NULL,
  Status        NVARCHAR(30)  NOT NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeGiftCards_CreatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT UQ_SafeGiftCards_PublicId UNIQUE (PublicId)
);

IF OBJECT_ID(N'dbo.SafeEvents', N'U') IS NULL
CREATE TABLE dbo.SafeEvents (
  Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  SourceApp     NVARCHAR(30)  NOT NULL,
  EventName     NVARCHAR(80)  NOT NULL,
  RequestId     INT           NULL,
  WorkOrderId   INT           NULL,
  Payload       NVARCHAR(MAX) NULL,
  CreatedAt     DATETIME2     NOT NULL CONSTRAINT DF_SafeEvents_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeRequests_Status')
  CREATE INDEX IX_SafeRequests_Status ON dbo.SafeRequests (Status);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeWorkOrders_RequestId')
  CREATE INDEX IX_SafeWorkOrders_RequestId ON dbo.SafeWorkOrders (RequestId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeMessages_ThreadId')
  CREATE INDEX IX_SafeMessages_ThreadId ON dbo.SafeMessages (ThreadId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeEvents_CreatedAt')
  CREATE INDEX IX_SafeEvents_CreatedAt ON dbo.SafeEvents (CreatedAt DESC);
GO

/* No demo rows. Clients, jobs, messages, leads and invoices are created from the portals. */
