CREATE PROCEDURE [dbo].[uspGetScreeningSections]
    @ScreeningID nvarchar(4)
AS
    SELECT 
        ScreeningSectionID, 
        ScreeningSectionName, 
        QuestionText, 
        ScreeningSectionShortName
    FROM dbo.ScreeningSection
    WHERE ScreeningID = @ScreeningID
    ORDER BY OrderIndex ASC
RETURN 0
