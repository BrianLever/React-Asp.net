CREATE PROCEDURE [dbo].[uspGetAllScoreLevels]
AS
    SELECT ScreeningSectionID, ScoreLevel, Name, Label
    FROM dbo.ScreeningScoreLevel
    ORDER BY ScreeningSectionID ASC, ScoreLevel ASC

RETURN 0
