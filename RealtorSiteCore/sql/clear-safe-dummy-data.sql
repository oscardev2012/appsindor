/* Wipe demo / leftover rows from the SAFE tables in IndorDB.
   Run this once if you already inserted the old sample clients and jobs.
   Tables themselves stay. */

USE IndorDB;
GO

DELETE FROM dbo.SafeMessages;
DELETE FROM dbo.SafeThreads;
DELETE FROM dbo.SafeEvents;
DELETE FROM dbo.SafeGiftCards;
DELETE FROM dbo.SafeInvoices;
DELETE FROM dbo.SafeWorkOrders;
DELETE FROM dbo.SafeEstimates;
DELETE FROM dbo.SafeRequests;
DELETE FROM dbo.SafeCampaigns;
DELETE FROM dbo.SafeLeads;
DELETE FROM dbo.SafePromotions;
DELETE FROM dbo.SafeProperties;
DELETE FROM dbo.SafeContacts;
GO
