CREATE PROCEDURE [dbo].[uspUpdateDrugOfChoice]
    @ScreeningResultID bigint,
    @PrimaryAnswer int,
    @SecondaryAnswer int,
    @TertiaryAnswer int

AS
BEGIN
    DECLARE @DrugOfChoiceSectionID char(5) = 'DOCH';
    DECLARE @sectionAnswer int = 0

    IF @PrimaryAnswer > 0
        SET @sectionAnswer = 1

    MERGE INTO dbo.ScreeningSectionResult target
    USING (
        VALUES(@ScreeningResultID,@DrugOfChoiceSectionID, @sectionAnswer, @sectionAnswer,@sectionAnswer)
        ) AS source(ScreeningResultID,ScreeningSectionID,AnswerValue,Score,ScoreLevel)
        ON target.ScreeningResultID = source.ScreeningResultID AND target.ScreeningSectionID = source.ScreeningSectionID
    WHEN MATCHED THEN  
        UPDATE SET AnswerValue = source.AnswerValue, Score = source.Score,  ScoreLevel = source.ScoreLevel
    WHEN NOT MATCHED BY TARGET THEN  
        INSERT (ScreeningResultID,ScreeningSectionID,AnswerValue,Score,ScoreLevel) 
        VALUES (source.ScreeningResultID,source.ScreeningSectionID,source.AnswerValue,source.Score, source.ScoreLevel); 

    ;

    MERGE INTO dbo.ScreeningSectionQuestionResult target
    USING ( VALUES
        (@ScreeningResultID, @DrugOfChoiceSectionID, 1, @PrimaryAnswer),
        (@ScreeningResultID, @DrugOfChoiceSectionID, 2, @SecondaryAnswer),
        (@ScreeningResultID, @DrugOfChoiceSectionID, 3, @TertiaryAnswer)

    ) AS source(ScreeningResultID,ScreeningSectionID,QuestionID,AnswerValue)
        ON target.ScreeningResultID = source.ScreeningResultID 
            AND target.ScreeningSectionID = source.ScreeningSectionID
            AND target.QuestionID = source.QuestionID
    WHEN MATCHED THEN  
        UPDATE SET AnswerValue = source.AnswerValue
    WHEN NOT MATCHED BY TARGET THEN  
        INSERT (ScreeningResultID,ScreeningSectionID,QuestionID,AnswerValue) 
        VALUES (source.ScreeningResultID,source.ScreeningSectionID,source.QuestionID,source.AnswerValue); 

    ;
END
RETURN 0
