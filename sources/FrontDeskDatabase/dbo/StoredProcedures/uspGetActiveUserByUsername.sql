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
