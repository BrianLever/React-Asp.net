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
