CREATE TABLE [dbo].[RoleTypes]
(
	[role_type_id] INT NOT NULL identity(1,1) PRIMARY KEY,
	[role_name] varchar(20),
	[role_description] varchar(50)
)
