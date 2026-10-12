/* SAFE Realtor — tabla para Crear mi plan de reparaciones. Correr en IndorDB. */
USE IndorDB;
GO

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
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SafeRealtorRepairPlans_Realtor')
  CREATE INDEX IX_SafeRealtorRepairPlans_Realtor ON dbo.SafeRealtorRepairPlans (RealtorEmail, UpdatedAt DESC);
GO
