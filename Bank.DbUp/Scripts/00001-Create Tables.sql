
CREATE TABLE [dbo].[Accounts]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[AccountNumber] NVARCHAR(MAX) NOT NULL,
	[AccountName] NVARCHAR(MAX) NOT NULL,
	[Password] NVARCHAR(MAX) NOT NULL,
	[Balance] DECIMAL(18,5) NOT NULL, 
	[CreatedDate] DATETIME NULL,
	[LastTransactionDate] DATETIME NULL,
	CONSTRAINT [PK_dbo.Accounts] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
)
GO

ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Accounts_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Accounts_LastTransactionDate]  DEFAULT (getutcdate()) FOR [LastTransactionDate]
GO

CREATE TABLE [dbo].[TransactionHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[AccountNumber] NVARCHAR(MAX) NOT NULL,
	[TransactionType] [varchar](50) NOT NULL,
	[TransactionInfo] [nvarchar](max) NULL,
	[TransactionAmount] [decimal](18, 5) NOT NULL,
	[CurrentBalance] [decimal](18, 5) NOT NULL,
	[TransactionDate] [datetime] NULL,
	CONSTRAINT [PK_TransactionHistory] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

ALTER TABLE [dbo].[TransactionHistory] ADD  CONSTRAINT [DF_TransactionHistory_TransactionDate]  DEFAULT (getutcdate()) FOR [TransactionDate]
GO

ALTER TABLE [dbo].[TransactionHistory]  WITH CHECK ADD  CONSTRAINT [FK_TransactionHistory_Accounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
GO

ALTER TABLE [dbo].[TransactionHistory] CHECK CONSTRAINT [FK_TransactionHistory_Accounts]
GO