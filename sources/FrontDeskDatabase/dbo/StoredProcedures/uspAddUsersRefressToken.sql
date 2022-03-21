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
