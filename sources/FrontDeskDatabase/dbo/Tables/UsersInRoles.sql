CREATE TABLE [dbo].[UsersInRoles](
	[Username] [nvarchar](255) NOT NULL,
	[Rolename] [varchar](255) NOT NULL,
	CONSTRAINT [PKUsersInRoles] PRIMARY KEY CLUSTERED ([Username] ASC,[Rolename] ASC),
	CONSTRAINT [FK_UsersInRoles_Roles] FOREIGN KEY([Rolename]) REFERENCES [dbo].[Roles] ([Rolename])
		ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT FK_UsersInRoles_Users FOREIGN KEY([Username]) REFERENCES [dbo].[Users] ([Username])
		ON UPDATE CASCADE ON DELETE CASCADE
) ;
