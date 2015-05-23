CREATE TABLE [dbo].[Administrators] (
    [UserId]    INT           IDENTITY (1, 1) NOT NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [Password]  VARCHAR (50)  NOT NULL,
    [FirstName] VARCHAR (50)  NOT NULL,
    [LastName]  VARCHAR (50)  NULL,
    [UserType]  VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

