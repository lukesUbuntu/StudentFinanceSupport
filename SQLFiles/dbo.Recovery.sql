CREATE TABLE [dbo].[Recovery]
(
	[recovery_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UserId]    INT,
	[recovery_key]  VARCHAR (20) NOT NULL,
	CONSTRAINT [FK_dbo.Administrators.Recovery_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Administrators] ([UserId])
)
