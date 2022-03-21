CREATE PROCEDURE [dbo].[uspGetQuestionPositiveScoreLevels]
    @ScreeningSectionID char(5),
    @QuestionID int
AS
    SELECT ScreeningSectionID, options.OptionValue, options.OptionText, options.OptionText
    FROM dbo.ScreeningSectionQuestion q 
        INNER JOIN dbo.AnswerScale scale ON scale.AnswerScaleID = q.AnswerScaleID
        INNER JOIN dbo.AnswerScaleOption options ON scale.AnswerScaleID = options.AnswerScaleID
    WHERE q.ScreeningSectionID = @ScreeningSectionID AND q.QuestionID = @QuestionID
        AND options.OptionValue > 0
    ORDER BY options.OptionValue

RETURN 0
