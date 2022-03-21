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