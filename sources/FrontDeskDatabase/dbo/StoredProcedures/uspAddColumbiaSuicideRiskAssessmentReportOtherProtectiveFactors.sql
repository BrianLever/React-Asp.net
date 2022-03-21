CREATE PROCEDURE dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherProtectiveFactors
    @ColumbiaReportID bigint
    ,@ItemID int
    ,@ProtectiveFactor nvarchar(max)
AS
INSERT INTO [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors] (
    [ColumbiaReportID]
    ,[ItemID]
    ,[ProtectiveFactor]
)
VALUES (
    @ColumbiaReportID
    ,@ItemID
    ,@ProtectiveFactor
)

RETURN 0
GO

