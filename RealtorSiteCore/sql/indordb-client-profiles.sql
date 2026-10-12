/* SAFE Realtor — ficha de fidelización del cliente (QA-026).
   Correr conectado a la base (IndorDB local o la base de Azure). No usa USE para que también sirva en Azure SQL.
   Se puede correr varias veces; solo crea lo que falta. */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

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
