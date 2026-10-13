/* SAFE — instalación completa para Azure SQL.
   Correr conectado a la base de Azure (la misma del ConnectionStrings__DefaultConnection).
   Sin CREATE DATABASE ni USE porque Azure SQL no los permite. Se puede correr varias veces sin dañar datos. */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* SAFE Ecosystem + Core + Operations  —  run this in IndorDB (SQL Server)
   All three portals read and write these same tables.
   Order: run the whole script once. Safe to re-run (IF NOT EXISTS). */


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

/* SAFE apps — tablas propias de SAFE Management, Field Team y Realtor en IndorDB (SQL Server).
   Correr DESPUÉS de indordb-safe-tables.sql. Se puede correr varias veces (IF NOT EXISTS).
   No modifica ni borra las tablas Safe* existentes. */


/* ---------- SAFE Management (Operations) ---------- */

IF OBJECT_ID(N'dbo.SafeOpsContacts', N'U') IS NULL
CREATE TABLE dbo.SafeOpsContacts (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeOpsContacts PRIMARY KEY,
  PublicId    AS CAST(CONCAT('CON-', CASE Role WHEN 'Realtor' THEN 'REL' WHEN 'Worker' THEN 'WRK' ELSE 'CLI' END, '-',
                CASE WHEN Id < 1000 THEN RIGHT(CONCAT('00', Id), 3) ELSE CAST(Id AS VARCHAR(10)) END) AS NVARCHAR(40)) PERSISTED,
  Role        NVARCHAR(20)   NOT NULL, -- Realtor | Client | Worker
  Name        NVARCHAR(160)  NOT NULL,
  Email       NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeOpsContacts_Email DEFAULT N'',
  Phone       NVARCHAR(40)   NOT NULL CONSTRAINT DF_SafeOpsContacts_Phone DEFAULT N'',
  Firm        NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeOpsContacts_Firm DEFAULT N'',
  Address     NVARCHAR(250)  NOT NULL CONSTRAINT DF_SafeOpsContacts_Address DEFAULT N'',
  Notes       NVARCHAR(1000) NOT NULL CONSTRAINT DF_SafeOpsContacts_Notes DEFAULT N'',
  CreatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsContacts_CreatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeOpsLeads', N'U') IS NULL
CREATE TABLE dbo.SafeOpsLeads (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeOpsLeads PRIMARY KEY,
  PublicId    AS CAST(CONCAT('LEAD-', CASE WHEN Id < 1000 THEN RIGHT(CONCAT('00', Id), 3) ELSE CAST(Id AS VARCHAR(10)) END) AS NVARCHAR(40)) PERSISTED,
  Name        NVARCHAR(160)  NOT NULL,
  Firm        NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeOpsLeads_Firm DEFAULT N'',
  Email       NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeOpsLeads_Email DEFAULT N'',
  Phone       NVARCHAR(40)   NOT NULL CONSTRAINT DF_SafeOpsLeads_Phone DEFAULT N'',
  Channel     NVARCHAR(40)   NOT NULL,
  Stage       NVARCHAR(20)   NOT NULL, -- New | Qualified | Conversation | Demo
  Notes       NVARCHAR(1000) NOT NULL CONSTRAINT DF_SafeOpsLeads_Notes DEFAULT N'',
  CreatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsLeads_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsLeads_UpdatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeOpsRequests', N'U') IS NULL
CREATE TABLE dbo.SafeOpsRequests (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeOpsRequests PRIMARY KEY,
  PublicId    AS CAST(CONCAT('REQ-M', CASE WHEN Id < 1000 THEN RIGHT(CONCAT('00', Id), 3) ELSE CAST(Id AS VARCHAR(10)) END) AS NVARCHAR(40)) PERSISTED,
  Address     NVARCHAR(250)  NOT NULL,
  City        NVARCHAR(120)  NOT NULL CONSTRAINT DF_SafeOpsRequests_City DEFAULT N'',
  Contact     NVARCHAR(160)  NOT NULL,
  Source      NVARCHAR(80)   NOT NULL,
  Type        NVARCHAR(1000) NOT NULL,
  Status      NVARCHAR(60)   NOT NULL,
  Priority    NVARCHAR(20)   NOT NULL,
  Due         DATE           NULL,
  Owner       NVARCHAR(80)   NOT NULL,
  Items       INT            NOT NULL CONSTRAINT DF_SafeOpsRequests_Items DEFAULT 0,
  Value       DECIMAL(18,2)  NOT NULL CONSTRAINT DF_SafeOpsRequests_Value DEFAULT 0,
  ApprovedAt  DATETIME2      NULL,
  CreatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsRequests_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsRequests_UpdatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeOpsEstimates', N'U') IS NULL
CREATE TABLE dbo.SafeOpsEstimates (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeOpsEstimates PRIMARY KEY,
  PublicId    AS CAST(CONCAT('EST-', CASE WHEN Id < 1000 THEN RIGHT(CONCAT('00', Id), 3) ELSE CAST(Id AS VARCHAR(10)) END) AS NVARCHAR(40)) PERSISTED,
  RequestId   NVARCHAR(40)   NULL,     -- PublicId de la solicitud (SafeRequests o SafeOpsRequests)
  Address     NVARCHAR(250)  NOT NULL,
  Client      NVARCHAR(160)  NOT NULL,
  Lines       NVARCHAR(MAX)  NOT NULL, -- JSON: [{serviceId, service, qty, unit, price}]
  Total       DECIMAL(18,2)  NOT NULL,
  Status      NVARCHAR(20)   NOT NULL, -- Draft | Sent | Approved | Declined
  Notes       NVARCHAR(2000) NOT NULL CONSTRAINT DF_SafeOpsEstimates_Notes DEFAULT N'',
  CreatedBy   NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeOpsEstimates_CreatedBy DEFAULT N'',
  CreatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsEstimates_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsEstimates_UpdatedAt DEFAULT SYSUTCDATETIME()
);

/* Reparaciones nuevas (Kind = custom, ItemId C0001...) y cambios a las del catálogo base (Kind = override, ItemId R0001...). */
IF OBJECT_ID(N'dbo.SafeOpsCatalogItems', N'U') IS NULL
CREATE TABLE dbo.SafeOpsCatalogItems (
  Seq         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeOpsCatalogItems PRIMARY KEY,
  ItemId      NVARCHAR(20)   NOT NULL CONSTRAINT UQ_SafeOpsCatalogItems_ItemId UNIQUE,
  Kind        NVARCHAR(20)   NOT NULL,
  Data        NVARCHAR(MAX)  NOT NULL, -- JSON con el mismo formato de repair-catalog.json
  UpdatedBy   NVARCHAR(160)  NULL,
  UpdatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsCatalogItems_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO

/* ---------- Field Team ---------- */

IF OBJECT_ID(N'dbo.SafeFieldWorkerRequests', N'U') IS NULL
CREATE TABLE dbo.SafeFieldWorkerRequests (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeFieldWorkerRequests PRIMARY KEY,
  Worker      NVARCHAR(160)  NOT NULL, -- email o nombre del trabajador en minúsculas
  WorkerName  NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeFieldWorkerRequests_WorkerName DEFAULT N'',
  Kind        NVARCHAR(20)   NOT NULL, -- new_property | estimate
  Address     NVARCHAR(250)  NOT NULL,
  ClientName  NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeFieldWorkerRequests_ClientName DEFAULT N'',
  ClientPhone NVARCHAR(40)   NOT NULL CONSTRAINT DF_SafeFieldWorkerRequests_ClientPhone DEFAULT N'',
  Notes       NVARCHAR(2000) NOT NULL CONSTRAINT DF_SafeFieldWorkerRequests_Notes DEFAULT N'',
  RepairId    NVARCHAR(20)   NOT NULL CONSTRAINT DF_SafeFieldWorkerRequests_RepairId DEFAULT N'',
  RepairName  NVARCHAR(200)  NOT NULL CONSTRAINT DF_SafeFieldWorkerRequests_RepairName DEFAULT N'',
  Low         DECIMAL(18,2)  NULL,
  High        DECIMAL(18,2)  NULL,
  Status      NVARCHAR(30)   NOT NULL,
  CreatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeFieldWorkerRequests_CreatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeFieldWorkerActivity', N'U') IS NULL
CREATE TABLE dbo.SafeFieldWorkerActivity (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeFieldWorkerActivity PRIMARY KEY,
  Worker      NVARCHAR(160)  NOT NULL,
  WorkerName  NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeFieldWorkerActivity_WorkerName DEFAULT N'',
  Kind        NVARCHAR(20)   NOT NULL, -- checkin | access | photos | change
  WorkOrderId NVARCHAR(40)   NOT NULL CONSTRAINT DF_SafeFieldWorkerActivity_WorkOrderId DEFAULT N'',
  Address     NVARCHAR(250)  NOT NULL,
  Detail      NVARCHAR(400)  NOT NULL CONSTRAINT DF_SafeFieldWorkerActivity_Detail DEFAULT N'',
  Body        NVARCHAR(2000) NOT NULL CONSTRAINT DF_SafeFieldWorkerActivity_Body DEFAULT N'',
  Photos      NVARCHAR(MAX)  NOT NULL CONSTRAINT DF_SafeFieldWorkerActivity_Photos DEFAULT N'[]', -- JSON con imágenes data:image
  CreatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeFieldWorkerActivity_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO

/* ---------- Safe Realtor ---------- */

IF OBJECT_ID(N'dbo.SafeRealtorClientServices', N'U') IS NULL
CREATE TABLE dbo.SafeRealtorClientServices (
  RealtorEmail NVARCHAR(160) NOT NULL,
  ClientKey    NVARCHAR(160) NOT NULL, -- email del cliente o nombre en minúsculas
  Name         NVARCHAR(160) NOT NULL,
  Email        NVARCHAR(160) NOT NULL CONSTRAINT DF_SafeRealtorClientServices_Email DEFAULT N'',
  Phone        NVARCHAR(40)  NOT NULL CONSTRAINT DF_SafeRealtorClientServices_Phone DEFAULT N'',
  Address      NVARCHAR(250) NOT NULL CONSTRAINT DF_SafeRealtorClientServices_Address DEFAULT N'',
  Services     NVARCHAR(MAX) NOT NULL, -- JSON: [{id, service, category}]
  UpdatedAt    DATETIME2     NOT NULL CONSTRAINT DF_SafeRealtorClientServices_UpdatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT PK_SafeRealtorClientServices PRIMARY KEY (RealtorEmail, ClientKey)
);

IF OBJECT_ID(N'dbo.SafeRealtorGiftCards', N'U') IS NULL
CREATE TABLE dbo.SafeRealtorGiftCards (
  Id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeRealtorGiftCards PRIMARY KEY,
  RealtorEmail NVARCHAR(160) NOT NULL,
  ClientName   NVARCHAR(160) NOT NULL,
  Address      NVARCHAR(250) NOT NULL CONSTRAINT DF_SafeRealtorGiftCards_Address DEFAULT N'',
  Firm         NVARCHAR(160) NOT NULL CONSTRAINT DF_SafeRealtorGiftCards_Firm DEFAULT N'',
  Amount       INT           NOT NULL,
  Code         NVARCHAR(40)  NOT NULL,
  Logo         NVARCHAR(MAX) NOT NULL, -- imagen data:image
  CreatedAt    DATETIME2     NOT NULL CONSTRAINT DF_SafeRealtorGiftCards_CreatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeRealtorProfiles', N'U') IS NULL
CREATE TABLE dbo.SafeRealtorProfiles (
  RealtorEmail NVARCHAR(160) NOT NULL CONSTRAINT PK_SafeRealtorProfiles PRIMARY KEY,
  Logo         NVARCHAR(MAX) NOT NULL CONSTRAINT DF_SafeRealtorProfiles_Logo DEFAULT N'',
  UpdatedAt    DATETIME2     NOT NULL CONSTRAINT DF_SafeRealtorProfiles_UpdatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeRealtorChannelConnections', N'U') IS NULL
CREATE TABLE dbo.SafeRealtorChannelConnections (
  RealtorEmail NVARCHAR(160) NOT NULL,
  Channel      NVARCHAR(30)  NOT NULL,
  Account      NVARCHAR(200) NOT NULL,
  ConnectedAt  DATETIME2     NOT NULL CONSTRAINT DF_SafeRealtorChannelConnections_ConnectedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT PK_SafeRealtorChannelConnections PRIMARY KEY (RealtorEmail, Channel)
);

IF OBJECT_ID(N'dbo.SafeRealtorChannelMessages', N'U') IS NULL
CREATE TABLE dbo.SafeRealtorChannelMessages (
  Id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeRealtorChannelMessages PRIMARY KEY,
  RealtorEmail NVARCHAR(160) NOT NULL,
  Channel      NVARCHAR(30)  NOT NULL,
  ClientKey    NVARCHAR(160) NOT NULL,
  ClientName   NVARCHAR(160) NOT NULL,
  Destination  NVARCHAR(200) NOT NULL CONSTRAINT DF_SafeRealtorChannelMessages_Destination DEFAULT N'',
  Body         NVARCHAR(MAX) NOT NULL,
  CreatedAt    DATETIME2     NOT NULL CONSTRAINT DF_SafeRealtorChannelMessages_CreatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeRealtorTipSends', N'U') IS NULL
CREATE TABLE dbo.SafeRealtorTipSends (
  Id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeRealtorTipSends PRIMARY KEY,
  RealtorEmail NVARCHAR(160) NOT NULL,
  TemplateId   NVARCHAR(60)  NOT NULL,
  Title        NVARCHAR(200) NOT NULL,
  Body         NVARCHAR(MAX) NOT NULL,
  Channel      NVARCHAR(30)  NOT NULL,
  SendDate     NVARCHAR(20)  NOT NULL CONSTRAINT DF_SafeRealtorTipSends_SendDate DEFAULT N'',
  Status       NVARCHAR(20)  NOT NULL,
  Recipients   NVARCHAR(MAX) NOT NULL, -- JSON: [{name, email, phone, address, body, services}]
  CreatedAt    DATETIME2     NOT NULL CONSTRAINT DF_SafeRealtorTipSends_CreatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeRealtorRepairPlans', N'U') IS NULL
CREATE TABLE dbo.SafeRealtorRepairPlans (
  Id               INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeRealtorRepairPlans PRIMARY KEY,
  RealtorEmail     NVARCHAR(160)  NOT NULL,
  ClientName       NVARCHAR(160)  NOT NULL,
  ClientEmail      NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_ClientEmail DEFAULT N'',
  Address          NVARCHAR(250)  NOT NULL,
  Stage            NVARCHAR(60)   NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_Stage DEFAULT N'',
  TargetClose      NVARCHAR(20)   NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_TargetClose DEFAULT N'',
  Goal             NVARCHAR(60)   NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_Goal DEFAULT N'',
  Items            NVARCHAR(MAX)  NOT NULL, -- JSON: [{id, service, category, qty, unit, techHours, low, high, materials, pricingModel, finding, quote}]
  Pricing          NVARCHAR(20)   NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_Pricing DEFAULT N'fixed',
  HourlyRate       DECIMAL(10,2)  NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_HourlyRate DEFAULT 0,
  Credit           INT            NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_Credit DEFAULT 0,
  MaxAuthorization DECIMAL(12,2)  NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_MaxAuthorization DEFAULT 0,
  PreferredDate    NVARCHAR(20)   NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_PreferredDate DEFAULT N'',
  PreferredWindow  NVARCHAR(40)   NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_PreferredWindow DEFAULT N'',
  AccessNotes      NVARCHAR(1000) NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_AccessNotes DEFAULT N'',
  Status           NVARCHAR(30)   NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_Status DEFAULT N'Draft',
  RequestId        NVARCHAR(40)   NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_RequestId DEFAULT N'',
  Step             INT            NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_Step DEFAULT 1,
  CreatedAt        DATETIME2      NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt        DATETIME2      NOT NULL CONSTRAINT DF_SafeRealtorRepairPlans_UpdatedAt DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID(N'dbo.SafeRealtorClientProfiles', N'U') IS NULL
CREATE TABLE dbo.SafeRealtorClientProfiles (
  RealtorEmail     NVARCHAR(160)  NOT NULL,
  ClientKey        NVARCHAR(160)  NOT NULL, -- email del cliente o nombre en minúsculas
  Name             NVARCHAR(160)  NOT NULL,
  Email            NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_Email DEFAULT N'',
  Phone            NVARCHAR(40)   NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_Phone DEFAULT N'',
  Address          NVARCHAR(250)  NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_Address DEFAULT N'',
  Side             NVARCHAR(20)   NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_Side DEFAULT N'', -- Buyer | Seller | Both
  ClosingDate      DATE           NULL,
  Birthday         DATE           NULL,
  PreferredChannel NVARCHAR(30)   NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_PreferredChannel DEFAULT N'',
  Household        NVARCHAR(400)  NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_Household DEFAULT N'',
  Interests        NVARCHAR(400)  NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_Interests DEFAULT N'',
  Notes            NVARCHAR(2000) NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_Notes DEFAULT N'',
  Consent          BIT            NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_Consent DEFAULT 0,
  CreatedAt        DATETIME2      NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt        DATETIME2      NOT NULL CONSTRAINT DF_SafeRealtorClientProfiles_UpdatedAt DEFAULT SYSUTCDATETIME(),
  CONSTRAINT PK_SafeRealtorClientProfiles PRIMARY KEY (RealtorEmail, ClientKey)
);
GO

/* ---------- Índices ---------- */

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UQ_SafeOpsContacts_PublicId')
  CREATE UNIQUE INDEX UQ_SafeOpsContacts_PublicId ON dbo.SafeOpsContacts (PublicId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UQ_SafeOpsLeads_PublicId')
  CREATE UNIQUE INDEX UQ_SafeOpsLeads_PublicId ON dbo.SafeOpsLeads (PublicId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UQ_SafeOpsRequests_PublicId')
  CREATE UNIQUE INDEX UQ_SafeOpsRequests_PublicId ON dbo.SafeOpsRequests (PublicId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UQ_SafeOpsEstimates_PublicId')
  CREATE UNIQUE INDEX UQ_SafeOpsEstimates_PublicId ON dbo.SafeOpsEstimates (PublicId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeOpsContacts_Role')
  CREATE INDEX IX_SafeOpsContacts_Role ON dbo.SafeOpsContacts (Role);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeFieldWorkerRequests_Worker')
  CREATE INDEX IX_SafeFieldWorkerRequests_Worker ON dbo.SafeFieldWorkerRequests (Worker, Id DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeFieldWorkerActivity_Worker')
  CREATE INDEX IX_SafeFieldWorkerActivity_Worker ON dbo.SafeFieldWorkerActivity (Worker, Id DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeRealtorGiftCards_Realtor')
  CREATE INDEX IX_SafeRealtorGiftCards_Realtor ON dbo.SafeRealtorGiftCards (RealtorEmail, Id DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeRealtorChannelMessages_Realtor')
  CREATE INDEX IX_SafeRealtorChannelMessages_Realtor ON dbo.SafeRealtorChannelMessages (RealtorEmail, Id);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeRealtorTipSends_Realtor')
  CREATE INDEX IX_SafeRealtorTipSends_Realtor ON dbo.SafeRealtorTipSends (RealtorEmail, Id DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeRealtorRepairPlans_Realtor')
  CREATE INDEX IX_SafeRealtorRepairPlans_Realtor ON dbo.SafeRealtorRepairPlans (RealtorEmail, UpdatedAt DESC);
GO

-- Perfiles del Equipo de campo, servicios por solicitud de campo y facturas de Management.
-- Compatible con Azure SQL (sin USE). Se puede correr más de una vez.

IF OBJECT_ID(N'dbo.SafeFieldWorkerProfiles', N'U') IS NULL
CREATE TABLE dbo.SafeFieldWorkerProfiles (
  Email        NVARCHAR(160)  NOT NULL CONSTRAINT PK_SafeFieldWorkerProfiles PRIMARY KEY, -- en minúsculas
  Name         NVARCHAR(160)  NOT NULL,
  Company      NVARCHAR(200)  NOT NULL CONSTRAINT DF_SafeFieldWorkerProfiles_Company DEFAULT N'',
  Phone        NVARCHAR(20)   NOT NULL CONSTRAINT DF_SafeFieldWorkerProfiles_Phone DEFAULT N'',
  Specialties  NVARCHAR(1000) NOT NULL CONSTRAINT DF_SafeFieldWorkerProfiles_Specialties DEFAULT N'[]', -- JSON con categorías del catálogo
  PasswordHash NVARCHAR(200)  NOT NULL,
  CreatedAt    DATETIME2      NOT NULL CONSTRAINT DF_SafeFieldWorkerProfiles_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt    DATETIME2      NOT NULL CONSTRAINT DF_SafeFieldWorkerProfiles_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO

IF OBJECT_ID(N'dbo.SafeFieldWorkerRequestItems', N'U') IS NULL
CREATE TABLE dbo.SafeFieldWorkerRequestItems (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeFieldWorkerRequestItems PRIMARY KEY,
  RequestId   INT            NOT NULL, -- dbo.SafeFieldWorkerRequests.Id
  RepairId    NVARCHAR(20)   NOT NULL,
  RepairName  NVARCHAR(200)  NOT NULL,
  Price       DECIMAL(18,2)  NOT NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeFieldWorkerRequestItems_RequestId')
CREATE INDEX IX_SafeFieldWorkerRequestItems_RequestId ON dbo.SafeFieldWorkerRequestItems (RequestId);
GO

IF OBJECT_ID(N'dbo.SafeOpsInvoices', N'U') IS NULL
CREATE TABLE dbo.SafeOpsInvoices (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeOpsInvoices PRIMARY KEY,
  PublicId    AS CAST(CONCAT('INV-M', CASE WHEN Id < 1000 THEN RIGHT(CONCAT('00', Id), 3) ELSE CAST(Id AS VARCHAR(10)) END) AS NVARCHAR(40)) PERSISTED,
  EstimateId  NVARCHAR(40)   NOT NULL CONSTRAINT DF_SafeOpsInvoices_EstimateId DEFAULT N'',
  Address     NVARCHAR(250)  NOT NULL,
  Client      NVARCHAR(160)  NOT NULL,
  Total       DECIMAL(18,2)  NOT NULL,
  Status      NVARCHAR(20)   NOT NULL, -- Open | Paid | Void
  DueDate     DATE           NULL,
  Notes       NVARCHAR(1000) NOT NULL CONSTRAINT DF_SafeOpsInvoices_Notes DEFAULT N'',
  CreatedBy   NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeOpsInvoices_CreatedBy DEFAULT N'',
  PaidAt      DATETIME2      NULL,
  CreatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsInvoices_CreatedAt DEFAULT SYSUTCDATETIME(),
  UpdatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeOpsInvoices_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO

IF OBJECT_ID(N'dbo.SafeAccessInstructions', N'U') IS NULL
CREATE TABLE dbo.SafeAccessInstructions (
  Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SafeAccessInstructions PRIMARY KEY,
  WorkOrderId INT            NOT NULL, -- dbo.SafeWorkOrders.Id
  Instruction NVARCHAR(500)  NOT NULL,
  SentBy      NVARCHAR(160)  NOT NULL CONSTRAINT DF_SafeAccessInstructions_SentBy DEFAULT N'',
  CreatedAt   DATETIME2      NOT NULL CONSTRAINT DF_SafeAccessInstructions_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeAccessInstructions_WorkOrderId')
CREATE INDEX IX_SafeAccessInstructions_WorkOrderId ON dbo.SafeAccessInstructions (WorkOrderId);
GO
