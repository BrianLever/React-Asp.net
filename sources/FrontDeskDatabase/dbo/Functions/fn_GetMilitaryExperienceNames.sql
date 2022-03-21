CREATE FUNCTION [dbo].[fn_GetMilitaryExperienceNames]
(
    @idValues varchar(32)
)
RETURNS  varchar(4000)
AS
BEGIN
    DECLARE @str NVARCHAR(MAX)

    DECLARE @Delimiter CHAR(1) = ','

    SELECT @str = COALESCE(@str + @Delimiter,'') + Name 
    FROM dbo.MilitaryExperience WHERE ID IN (
        SELECT [value] FROM dbo.fn_IntListToTable(@IdValues)
    )

    RETURN RTRIM(LTRIM(@str))
END
GO
