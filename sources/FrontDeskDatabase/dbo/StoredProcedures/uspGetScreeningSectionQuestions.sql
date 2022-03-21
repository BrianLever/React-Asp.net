CREATE PROCEDURE [dbo].[uspGetScreeningSectionQuestions]
    @ScreeningID nvarchar(4)
AS
    SELECT 
    q.QuestionID, 
    q.ScreeningSectionID, 
    q.PreambleText, 
    q.QuestionText, 
    q.AnswerScaleID,
    q.IsMainQuestion,
    q.OnlyWhenPossitive
    FROM dbo.ScreeningSectionQuestion q 
        INNER JOIN ScreeningSection s ON q.ScreeningSectionID = s.ScreeningSectionID
    WHERE s.ScreeningID = @ScreeningID
    ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, q.QuestionID ASC
RETURN 0
