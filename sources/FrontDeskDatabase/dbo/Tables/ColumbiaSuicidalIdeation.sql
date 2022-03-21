-- Suicidal Ideation
CREATE TABLE [dbo].[ColumbiaSuicidalIdeation]
(
    ColumbiaReportID bigint NOT NULL, /*foreign key*/
    QuestionID int NOT NULL, /* unique question number */
    LifetimeMostSucidal int NULL, /* 0 - No, 1 - Yes */
    PastLastMonth int NULL, /* 0 - No, 1 - Yes */
    [Description] nvarchar(max) NULL,
    
    CONSTRAINT PK__ColumbiaSuicidalIdeation PRIMARY KEY CLUSTERED (ColumbiaReportID, QuestionID),
     
    CONSTRAINT FK__ColumbiaSuicidalIdeation__ColumbiaSuicideReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideReport (ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)

GO