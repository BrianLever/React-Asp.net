CREATE FUNCTION [dbo].[fn_IntListToTable] (@InputString varchar(4000))
RETURNS  @OutputTable TABLE([value] BIGINT)
AS
BEGIN
    DECLARE @val VARCHAR(10),
    @Delimiter nvarchar(1) = ',';

    WHILE LEN(@InputString) > 0
    BEGIN
        SET @val = LEFT(@inputString, 
            ISNULL(NULLIF(CHARINDEX(@Delimiter, @InputString) - 1, -1),
            LEN(@InputString)))

        SET @InputString = SUBSTRING(@InputString,
                                     ISNULL(NULLIF(CHARINDEX(@Delimiter, @InputString), 0),
                                     LEN(@InputString)) + 1, LEN(@InputString))

        INSERT INTO @OutputTable([value]) VALUES (@val)
    END

    RETURN
END
GO