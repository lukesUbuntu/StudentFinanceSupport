CREATE TABLE [dbo].[StudentRegistration] (
    [Student_ID]         VARCHAR (20) NOT NULL,
    [FirstName]          VARCHAR (20) NOT NULL,
    [LastName]           VARCHAR (20) NULL,
    [Gender]             VARCHAR (10) NOT NULL,
    [DOB]                DATE     NOT NULL,
    [Address1]           VARCHAR (50) NULL,
    [Accomodition_Type]  VARCHAR (50) NULL,
    [Phone]              VARCHAR (20) NULL,
    [Mobile]             VARCHAR (20) NULL,
    [Email]              VARCHAR (50) NULL,
    [Marital_Status]     VARCHAR (50) NULL,
    [Contact]            VARCHAR (50) NULL,
    [Main_Ethnicity]     VARCHAR (50) NULL,
    [id_faculty]         INT          NOT NULL,
    [id_courses]         INT          NOT NULL,
    [Detailed_Ethnicity] VARCHAR (50) NULL,
    [campus]             VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Student_ID] ASC),
    CONSTRAINT [FK_id_faculty] FOREIGN KEY ([id_faculty]) REFERENCES [dbo].[Faculty] ([id_faculty])
);

