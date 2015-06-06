CREATE TABLE [dbo].[StudentVouchers] (
    [id_student_vouchers] INT          IDENTITY (1, 1) NOT NULL,
    [student_ID]          VARCHAR (20) NOT NULL,
    [grant_type_id]       INT          NULL,
    [GrantDescription]    VARCHAR (50) NULL,
    [GrantValue]          FLOAT (53)   NOT NULL,
    [DateOfIssue]         DATETIME     CONSTRAINT [DF_Issue] DEFAULT (getdate()) NOT NULL,
    [KuhaFunds]           FLOAT (53)   NULL,
    CONSTRAINT [PK_dbo.StudentVouchers] PRIMARY KEY CLUSTERED ([id_student_vouchers] ASC),
    CONSTRAINT [FK_dbo.StudentRegistration_dbo.student_ID] FOREIGN KEY ([student_ID]) REFERENCES [dbo].[StudentRegistration] ([Student_ID]),
    CONSTRAINT [FK_dbo.GrantType_dbo.grant_type_id] FOREIGN KEY ([grant_type_id]) REFERENCES [dbo].[GrantType] ([grant_type_id])
);

