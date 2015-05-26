CREATE TABLE [dbo].[Administrators] (
    [UserId]    INT           IDENTITY (1, 1) NOT NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [Password]  VARCHAR (250) NOT NULL,
    [FirstName] VARCHAR (50)  NOT NULL,
    [LastName]  VARCHAR (50)  NULL,
    [role_id]   INT           NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
    
);

