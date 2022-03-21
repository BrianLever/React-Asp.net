

-- Delete Minimal Level
update ScreeningSectionResult SET ScoreLevel = 0 Where ScreeningSectionID = 'PHQ-9' AND ScoreLevel = 1;

delete from ScreeningScoreLevel WHERE ScreeningSectionID = 'PHQ-9' AND ScoreLevel = 1;

--
update ScreeningScoreLevel set Indicates = '' where ScreeningSectionID = 'HITS'


IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'DbVersion')
BEGIN
CREATE TABLE DbVersion
(
   DbVersion varchar(32) NOT NULL CONSTRAINT PK_DbVersion PRIMARY KEY CLUSTERED,
   UpdatedOnUTC datetime CONSTRAINT DF_DbVersion_UpdatedOn DEFAULT (GETUTCDATE())
)

END

GRANT SELECT ON DbVersion TO PUBLIC

-----------
INSERT INTO dbo.DbVersion(DbVersion) VALUES('4.0.1.0');