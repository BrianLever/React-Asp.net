/* OTHER RISK FACTORS */
CREATE TABLE [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors]
(
    ColumbiaReportID bigint NOT NULL /*foreign key*/,
    ItemID int NOT NULL, /* unique question number */
    RiskFactor nvarchar (max),


    CONSTRAINT PK__ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors 
        PRIMARY KEY CLUSTERED (ColumbiaReportID, ItemID),
     
    CONSTRAINT FK__ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors__ColumbiaSuicideRiskAssessmentReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideRiskAssessmentReport (ColumbiaReportID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)