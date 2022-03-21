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