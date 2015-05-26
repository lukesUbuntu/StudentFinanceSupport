CREATE TABLE [dbo].[Roles]
(
	[role_id] INT NOT NULL identity(1,1)  PRIMARY KEY,
	[UserId]  INT NOT NULL,
	[role_type] varchar(10),
	CONSTRAINT [FK_dbo.Administrators.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Administrators] ([UserId])
)
