CREATE TABLE [dbo].[UsersRefreshToken]
(
    [Id] uniqueidentifier NOT NULL,
    [UserID] int NOT NULL,
    [Token] nvarchar(64) NOT NULL,
    [Created] datetime NOT NULL,
    [Expires] datetime NOT NULL,
    [CreatedByIp] varchar(16) NULL,
    [Revoked] datetime NULL,
    [RevokedByIp] varchar(16) NULL,
    [ReplacedByToken] nvarchar(64) NULL,
    [ReasonRevoked] nvarchar(128) NULL,

    CONSTRAINT PK__UsersRefreshToken PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ__UsersRefreshToken__Token UNIQUE (Token, UserID),
    CONSTRAINT FK__UsersRefreshToken__Users FOREIGN KEY(UserID)
        REFERENCES [dbo].[Users] ([PKID])
        ON UPDATE CASCADE
        ON DELETE CASCADE,
)
