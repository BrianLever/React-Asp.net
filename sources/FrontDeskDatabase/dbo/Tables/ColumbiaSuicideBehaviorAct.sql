CREATE TABLE [dbo].[ColumbiaSuicideBehaviorAct]
(
    ColumbiaReportID bigint NOT NULL /*foreign key*/,
    QuestionID int NOT NULL, /* unique question number */
    LifetimeLevel int NULL, /* 0 - No, 1 - Yes */
    LifetimeCount int NULL,

    PastThreeMonths int NULL, /* 0 - No, 1 - Yes */
    PastThreeMonthsCount int NULL,

    [Description] nvarchar(max) NULL,


    CONSTRAINT PK_ColumbiaSuicideBehaviorAct PRIMARY KEY CLUSTERED (ColumbiaReportID, QuestionID),
     
    CONSTRAINT FK__ColumbiaSuicideBehaviorAct__ColumbiaSuicideReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideReport (ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
