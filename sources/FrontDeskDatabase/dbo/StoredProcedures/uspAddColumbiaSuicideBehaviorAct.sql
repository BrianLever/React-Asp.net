CREATE PROCEDURE dbo.uspAddColumbiaSuicideBehaviorAct
    @ColumbiaReportID bigint
    ,@QuestionID int
    ,@LifetimeLevel int
    ,@LifetimeCount int
    ,@PastThreeMonths int
    ,@PastThreeMonthsCount int
    ,@Description nvarchar(max)
AS
INSERT INTO [dbo].[ColumbiaSuicideBehaviorAct] (
    [ColumbiaReportID]
    ,[QuestionID]
    ,[LifetimeLevel]
    ,[LifetimeCount]
    ,[PastThreeMonths]
    ,[PastThreeMonthsCount]
    ,[Description]
)
VALUES (
    @ColumbiaReportID
    ,@QuestionID
    ,@LifetimeLevel
    ,@LifetimeCount
    ,@PastThreeMonths
    ,@PastThreeMonthsCount
    ,@Description
)

RETURN 0
GO

