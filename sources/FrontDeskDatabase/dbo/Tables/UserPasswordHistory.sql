CREATE TABLE [dbo].[UserPasswordHistory](
	[PKID] [int] NOT NULL,
	[Password1] [nvarchar](128) NOT NULL,
	[Password2] [nvarchar](128) NULL,
	CONSTRAINT [PK_UserPasswordHistory] PRIMARY KEY CLUSTERED ([PKID] ASC),
	CONSTRAINT [FK_UserPasswordHistory_Users] FOREIGN KEY([PKID])
		REFERENCES [dbo].[Users] ([PKID])
		ON UPDATE CASCADE ON DELETE CASCADE
)
GO

