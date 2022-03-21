CREATE FUNCTION [dbo].[fn_GetFullName]
(
	@LastName nvarchar(255), 
	@FirstName nvarchar(255), 
	@MiddleName nvarchar(255)
)
RETURNS nvarchar(max)
WITH SCHEMABINDING
AS
BEGIN
	DECLARE @Result nvarchar(max);
	SET @Result = ISNULL(@FirstName, '');

	IF LEN(ISNULL(@MiddleName, '')) > 0
	BEGIN
		IF LEN(@Result) > 0
			SET @Result = @Result + ' ';

		SET @Result = @Result + @MiddleName;
	END

	IF LEN(ISNULL(@LastName, '')) > 0
	BEGIN
		IF LEN(@Result) > 0
			SET @Result = @Result + ' ';

		SET @Result = @Result + @LastName;
	END

	RETURN @Result;
END
GO


