CREATE TABLE [dbo].[Users](
	[PKID] int NOT NULL IDENTITY(1, 1),
	[Username] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[Comment] [ntext] NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordQuestion] [nvarchar](255) NULL,
	[PasswordAnswer] [nvarchar](255) NULL,
	[IsApproved] [bit] NULL,
	[LastActivityDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[IsOnLine] [bit] NULL,
	[IsLockedOut] [bit] NULL,
	[LastLockedOutDate] [datetime] NULL,
	[FailedPasswordAttemptCount] [int] NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NULL,
	[FailedPasswordAnswerAttemptCount] [int] NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL
	CONSTRAINT [PK__Users] PRIMARY KEY NONCLUSTERED 
	(
		[PKID] ASC
	),
	CONSTRAINT [UQ__Users] UNIQUE NONCLUSTERED 
	(
		[Username] ASC
	)
);
GO
