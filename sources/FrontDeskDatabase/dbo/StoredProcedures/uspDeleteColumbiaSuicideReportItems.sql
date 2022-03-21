CREATE PROCEDURE [dbo].[uspDeleteColumbiaSuicideReportItems]
    @ColumbiaReportID bigint
AS
BEGIN
    DELETE FROM [dbo].[ColumbiaSuicidalIdeation] WHERE ColumbiaReportID = @ColumbiaReportID;
    DELETE FROM [dbo].[ColumbiaIntensityIdeation] WHERE ColumbiaReportID = @ColumbiaReportID;
    DELETE FROM [dbo].[ColumbiaSuicideBehaviorAct] WHERE ColumbiaReportID = @ColumbiaReportID;

END
RETURN 0
