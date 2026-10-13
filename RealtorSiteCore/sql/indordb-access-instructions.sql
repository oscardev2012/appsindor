/* SAFE — instrucción de acceso segura a la propiedad (código de lockbox).
   Correr conectado a la base (IndorDB local o la base de Azure). No usa USE para que también sirva en Azure SQL.
   Se puede correr varias veces; solo crea lo que falta. */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
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
