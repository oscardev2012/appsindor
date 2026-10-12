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
