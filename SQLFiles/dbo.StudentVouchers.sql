CREATE TABLE [dbo].[StudentVouchers] (
	[id_student_vouchers]    INT          IDENTITY (1, 1) NOT NULL,
    [student_ID]       VARCHAR (20) NOT NULL,
    [GrantType]        VARCHAR (50) NOT NULL,
    [GrantDescription] VARCHAR (50) NOT NULL,
    [GrantValue]       FLOAT (53)   NOT NULL,
    [DateOfIssue]      DATETIME     CONSTRAINT [DF_Issue] DEFAULT (getdate()) NOT NULL,
    [KuhaFunds]        FLOAT (53)   NULL
	 CONSTRAINT [PK_dbo.StudentVouchers] PRIMARY KEY CLUSTERED ([id_student_vouchers] ASC)
   
);

