CREATE TABLE [dbo].[GrantType]
(
	[grant_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[grant_name] VARCHAR(50) NOT NULL,
	[grant_value] BIT CONSTRAINT [DF_grant_value] DEFAULT (1) NOT NULL,
	[grant_description] BIT CONSTRAINT [DF_grant_description] DEFAULT (0) NOT NULL,
	[grant_koha] BIT CONSTRAINT [DF_grant_koha] DEFAULT (0) NOT NULL,
	[system_asset] BIT CONSTRAINT [DF_system_asset] DEFAULT (0) NOT NULL,
	
)
