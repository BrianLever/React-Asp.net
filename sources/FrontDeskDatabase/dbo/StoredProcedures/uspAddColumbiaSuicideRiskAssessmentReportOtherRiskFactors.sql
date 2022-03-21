CREATE PROCEDURE dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherRiskFactors
    @ColumbiaReportID bigint
    ,@ItemID int
    ,@RiskFactor nvarchar(max)
AS
INSERT INTO [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors] (
    [ColumbiaReportID]
    ,[ItemID]
    ,[RiskFactor]
)
VALUES (
    @ColumbiaReportID
    ,@ItemID
    ,@RiskFactor
)

RETURN 0
GO

