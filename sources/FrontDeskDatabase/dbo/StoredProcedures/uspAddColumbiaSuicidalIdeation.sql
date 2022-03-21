CREATE PROCEDURE dbo.uspAddColumbiaSuicidalIdeation
    @ColumbiaReportID bigint
    ,@QuestionID int
    ,@LifetimeMostSucidal int
    ,@PastLastMonth int
    ,@Description nvarchar(max)
AS
    INSERT dbo.ColumbiaSuicidalIdeation (
        ColumbiaReportID
        ,QuestionID
        ,LifetimeMostSucidal
        ,PastLastMonth
        ,[Description]
    ) VALUES (
         @ColumbiaReportID
        ,@QuestionID
        ,@LifetimeMostSucidal
        ,@PastLastMonth
        ,@Description
    )

RETURN 0
GO

