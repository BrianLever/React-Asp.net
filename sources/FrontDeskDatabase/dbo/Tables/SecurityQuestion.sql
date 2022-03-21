CREATE TABLE [dbo].[SecurityQuestion](
		[QuestionID] [int] IDENTITY(1,1) NOT NULL,
		[QuestionText] [nvarchar](255) NOT NULL,
		CONSTRAINT [PK_SecurityQuestion] PRIMARY KEY CLUSTERED ([QuestionID] ASC)
	);
GO
