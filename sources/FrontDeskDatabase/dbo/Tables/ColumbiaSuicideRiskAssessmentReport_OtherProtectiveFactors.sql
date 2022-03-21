/* OTHER PROTECTIVE FACTORS */
CREATE TABLE [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors]
(
    ColumbiaReportID bigint NOT NULL /*foreign key*/,
    ItemID int NOT NULL, /* unique question number */
    ProtectiveFactor nvarchar (max),


    CONSTRAINT PK__ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors 
        PRIMARY KEY CLUSTERED (ColumbiaReportID, ItemID),
     
    CONSTRAINT FK__ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors__ColumbiaSuicideRiskAssessmentReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideRiskAssessmentReport (ColumbiaReportID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)

GO