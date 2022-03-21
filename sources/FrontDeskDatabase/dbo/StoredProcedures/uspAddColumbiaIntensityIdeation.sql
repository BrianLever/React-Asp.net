CREATE PROCEDURE dbo.uspAddColumbiaIntensityIdeation
    @ColumbiaReportID bigint
    ,@QuestionID int
    ,@LifetimeMostSevere int
    ,@RecentMostSevere int
AS
INSERT INTO [dbo].[ColumbiaIntensityIdeation] (
    [ColumbiaReportID]
    ,[QuestionID]
    ,[LifetimeMostSevere]
    ,[RecentMostSevere]
)
VALUES (
    @ColumbiaReportID
    ,@QuestionID
    ,@LifetimeMostSevere
    ,@RecentMostSevere
)

RETURN 0
GO

