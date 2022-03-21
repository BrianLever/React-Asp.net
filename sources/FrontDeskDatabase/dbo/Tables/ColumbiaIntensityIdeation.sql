CREATE TABLE [dbo].[ColumbiaIntensityIdeation]
(
    ColumbiaReportID bigint NOT NULL /*foreign key*/,
    QuestionID int NOT NULL, /* unique question number */
    LifetimeMostSevere int NULL,
    RecentMostSevere int NULL,

    CONSTRAINT PK__ColumbiaIntensityIdeation PRIMARY KEY CLUSTERED (ColumbiaReportID, QuestionID),
     
    CONSTRAINT FK__ColumbiaIntensityIdeation__ColumbiaSuicideReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideReport (ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)

GO
