
CREATE TABLE [dbo].[Accounts]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[AccountNumber] NVARCHAR(MAX) NOT NULL,
	[AccountName] NVARCHAR(MAX) NOT NULL,
	[Password] NVARCHAR(MAX) NOT NULL,
	[Balance] DECIMAL(12,5) NOT NULL, 
	[CreatedDate] DATETIME NULL,
	CONSTRAINT [PK_dbo.Accounts] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
)
GO

ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Accounts_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO