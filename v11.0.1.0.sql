IF OBJECT_ID('[dbo].[uspGetActiveUserByUsername]') IS NOT NULL
    DROP PROC [dbo].[uspGetActiveUserByUsername]
go


CREATE PROCEDURE [dbo].[uspGetActiveUserByUsername]
    @Username varchar(255)
AS
    SELECT ud.* 
      ,u.[Username]
      ,u.[Email]
      ,u.[Comment]
      ,u.[Password]
      ,u.[PasswordQuestion]
      ,u.[PasswordAnswer]
      ,u.[IsApproved]
      ,u.[LastActivityDate]
      ,u.[LastLoginDate]
      ,u.[LastPasswordChangedDate]
      ,u.[CreationDate]
      ,u.[IsOnLine]
      ,u.[IsLockedOut]
      ,u.[LastLockedOutDate]
      ,u.[FailedPasswordAttemptCount]
      ,u.[FailedPasswordAttemptWindowStart]
      ,u.[FailedPasswordAnswerAttemptCount]
      ,u.[FailedPasswordAnswerAttemptWindowStart]
      ,roles.Rolename
      ,ubl.BranchLocationID
    FROM Users u INNER JOIN dbo.UserDetails ud ON ud.UserID = u.PKID
        LEFT JOIN dbo.Users_BranchLocation ubl ON ud.UserID = ubl.UserID
        Left JOIN dbo.UsersInRoles ur ON u.UserName = ur.UserName
        CROSS APPLY (
            SELECT TOP 1 Rolename from UsersInRoles ur WHERE ur.Username = u.Username) as roles
    WHERE u.Username = @Username AND u.IsLockedOut = 0 AND ud.isBlock = 0
RETURN 0

GO
------------------
IF OBJECT_ID('[dbo].[UsersRefreshToken]') IS NOT NULL
BEGIN
SET NOEXEC ON

END

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




SET NOEXEC OFF
GO
-------------------------

IF OBJECT_ID('[dbo].[uspAddUsersRefressToken]') IS NOT NULL
    DROP PROC [dbo].uspAddUsersRefressToken
go


CREATE PROCEDURE [dbo].[uspAddUsersRefressToken]
@Id uniqueidentifier
,@UserID int
,@Token nvarchar(64)
,@Created datetime
,@Expires datetime
,@CreatedByIp varchar(16)
,@Revoked datetime
,@RevokedByIp varchar(16)
,@ReplacedByToken nvarchar(64)
,@ReasonRevoked nvarchar(128)

AS
    INSERT INTO [dbo].[UsersRefreshToken]
           ([Id]
           ,[UserID]
           ,[Token]
           ,[Created]
           ,[Expires]
           ,[CreatedByIp]
           ,[Revoked]
           ,[RevokedByIp]
           ,[ReplacedByToken]
           ,[ReasonRevoked])
     VALUES
           (@Id
           ,@UserID
           ,@Token
           ,@Created
           ,@Expires
           ,@CreatedByIp
           ,@Revoked
           ,@RevokedByIp
           ,@ReplacedByToken
           ,@ReasonRevoked
        )
RETURN 0

GO

-------------------------
IF OBJECT_ID('[dbo].[uspGetUsersRefressToken]') IS NOT NULL
    DROP PROC [dbo].uspGetUsersRefressToken
go


CREATE PROCEDURE [dbo].[uspGetUsersRefressToken]
    @Token nvarchar(64)
AS
SELECT [Id]
      ,[UserID]
      ,[Token]
      ,[Created]
      ,[Expires]
      ,[CreatedByIp]
      ,[Revoked]
      ,[RevokedByIp]
      ,[ReplacedByToken]
      ,[ReasonRevoked]
  FROM [dbo].[UsersRefreshToken]
  WHERE [Token] = @Token
GO
------------------

IF OBJECT_ID('[dbo].[uspRevokeAllUsersRefressToken]') IS NOT NULL
    DROP PROC [dbo].[uspRevokeAllUsersRefressToken]
GO


CREATE PROCEDURE [dbo].[uspRevokeAllUsersRefressToken]
 @UserID int
,@Revoked datetime
,@RevokedByIp varchar(16)
,@ReplacedByToken nvarchar(64)
,@ReasonRevoked nvarchar(128)

AS
    UPDATE [dbo].[UsersRefreshToken] SET 
        [Revoked] = @Revoked
        ,[RevokedByIp] = @RevokedByIp
        ,[ReplacedByToken] = @ReplacedByToken
        ,[ReasonRevoked] = @ReasonRevoked
    WHERE UserID = @UserID 
        AND Revoked IS NULL
        AND Expires >= @Revoked

GO

----
IF OBJECT_ID('[dbo].[uspRevokeUsersRefressToken]') IS NOT NULL
    DROP PROC [dbo].[uspRevokeUsersRefressToken]
GO


CREATE PROCEDURE [dbo].[uspRevokeUsersRefressToken]
 @Id uniqueidentifier
,@Revoked datetime
,@RevokedByIp varchar(16)
,@ReplacedByToken nvarchar(64)
,@ReasonRevoked nvarchar(128)

AS
    UPDATE [dbo].[UsersRefreshToken] SET 
        [Revoked] = @Revoked
        ,[RevokedByIp] = @RevokedByIp
        ,[ReplacedByToken] = @ReplacedByToken
        ,[ReasonRevoked] = @ReasonRevoked
    WHERE ID = @Id

GO
---------------------

IF OBJECT_ID('[dbo].[uspGetUserByID]') IS NOT NULL
    DROP PROC [dbo].[uspGetUserByID]
GO

CREATE PROCEDURE [dbo].[uspGetUserByID]
    @UserID int
AS
    SELECT ud.* 
      ,u.[Username]
      ,u.[Email]
      ,u.[Comment]
      ,u.[Password]
      ,u.[PasswordQuestion]
      ,u.[PasswordAnswer]
      ,u.[IsApproved]
      ,u.[LastActivityDate]
      ,u.[LastLoginDate]
      ,u.[LastPasswordChangedDate]
      ,u.[CreationDate]
      ,u.[IsOnLine]
      ,u.[IsLockedOut]
      ,u.[LastLockedOutDate]
      ,u.[FailedPasswordAttemptCount]
      ,u.[FailedPasswordAttemptWindowStart]
      ,u.[FailedPasswordAnswerAttemptCount]
      ,u.[FailedPasswordAnswerAttemptWindowStart]
      ,roles.Rolename
      ,ubl.BranchLocationID
    FROM Users u INNER JOIN dbo.UserDetails ud ON ud.UserID = u.PKID
        LEFT JOIN dbo.Users_BranchLocation ubl ON ud.UserID = ubl.UserID
        Left JOIN dbo.UsersInRoles ur ON u.UserName = ur.UserName
        CROSS APPLY (
            SELECT TOP 1 Rolename from UsersInRoles ur WHERE ur.Username = u.Username) as roles
    WHERE u.PKID = @UserID
RETURN 0


--------------------

IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '11.0.1.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('11.0.1.0');