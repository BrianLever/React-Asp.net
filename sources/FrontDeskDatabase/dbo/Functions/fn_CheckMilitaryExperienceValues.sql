CREATE FUNCTION [dbo].[fn_CheckMilitaryExperienceValues]
(
    @arrayString varchar(32)
)
RETURNS INT
AS
BEGIN
    declare @expectedCount int = 0,
    @actualCount int = 0;
    
    IF @arrayString IS NULL 
        RETURN 1;


    declare @IdValues table ([value] int);

    INSERT INTO @IdValues
    SELECT [value]
    FROM dbo.fn_IntListToTable(@arrayString);
    

    SET @expectedCount = (SELECT COUNT(*) FROM @IdValues);
    
    SET @actualCount = (
        SELECT COUNT(*)
        FROM @IdValues id INNER JOIN dbo.MilitaryExperience m ON id.value = m.ID
        );

    IF @actualCount = @expectedCount RETURN 1
    
    RETURN 0;
   
END
GO