CREATE TABLE [dbo].[Roles] (
    [role_id]      INT IDENTITY (1, 1) NOT NULL,
    [UserId]       INT NOT NULL,
    [role_type_id] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([role_id] ASC),
    CONSTRAINT [FK_dbo.RoleTypes.role_type_id] FOREIGN KEY ([role_type_id]) REFERENCES [dbo].[RoleTypes] ([role_type_id]),
	CONSTRAINT [FK_dbo.Administrators.UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Administrators] ([UserId])
);

