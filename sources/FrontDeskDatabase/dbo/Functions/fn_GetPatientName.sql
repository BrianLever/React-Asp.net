CREATE FUNCTION [dbo].[fn_GetPatientName](
	@LastName nvarchar(255), 
	@FirstName nvarchar(255), 
	@MiddleName nvarchar(255)
)
RETURNS nvarchar(max)
WITH SCHEMABINDING
AS
BEGIN

DECLARE @comma bit = 0; -- where comma was added
DECLARE @Result nvarchar(max);
SET @Result = ISNULL(@LastName, '');

IF LEN(ISNULL(@FirstName, '')) > 0
BEGIN
	IF LEN(@Result) > 0
	BEGIN
		SET @Result = @Result + ', ';
		SET @comma = 1;
		
		SET @Result = @Result + @FirstName;
	END
	
END	

IF LEN(ISNULL(@MiddleName, '')) > 0
BEGIN
	IF LEN(@Result) > 0
	BEGIN
		IF @comma = 1
			SET @Result = @Result + ' ';
		ELSE 
		SET @Result = @Result + ', ';
		
		SET @Result = @Result + @MiddleName;
	END

	
END

RETURN @Result;
END
GO

