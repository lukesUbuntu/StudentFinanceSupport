CREATE TABLE [dbo].[Roles]
(
	[role_id] INT NOT NULL identity(1,1)  PRIMARY KEY,
	[UserId]  INT NOT NULL,
	[role_type_id] INT NOT NULL,
	CONSTRAINT [FK_dbo.RoleTypes.role_type_id] FOREIGN KEY ([role_type_id]) REFERENCES [dbo].[RoleTypes] ([role_type_id]),
	CONSTRAINT [FK_dbo.Administrators.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Administrators] ([UserId])
)
