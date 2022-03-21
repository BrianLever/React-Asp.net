--HITS
Update dbo.ScreeningScoreLevel SET
     Name = 'NEGATIVE'
     ,Indicates = 'No problems reported. Review with patient (if possible)'
Where ScreeningSectionID = 'HITS' AND ScoreLevel = 0   

Update dbo.ScreeningScoreLevel SET
     Name = 'Evidence of CURRENT PROBLEM'
     ,Indicates = 'Need for immediate investigation and/or referral'
Where ScreeningSectionID = 'HITS' AND ScoreLevel = 1
GO

--------------
INSERT INTO dbo.DbVersion(DbVersion) VALUES('4.0.2.0');

