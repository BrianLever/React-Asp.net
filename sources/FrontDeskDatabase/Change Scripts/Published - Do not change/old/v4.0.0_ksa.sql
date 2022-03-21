-- Screening Frequency feature

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningFrequency')
	BEGIN
		DROP  Table dbo.ScreeningFrequency
	END
GO

CREATE TABLE dbo.ScreeningFrequency
(
	ID varchar(16) NOT NULL,
	Frequency int NOT NULL,
	LastModifiedDateUTC datetime NOT NULL,
	CONSTRAINT PK_ScreeningFrequency PRIMARY KEY(ID ASC)
)
GO


GRANT SELECT,INSERT, UPDATE ON dbo.ScreeningFrequency TO frontdesk_appuser

GO 


delete from ScreeningFrequency;

insert into ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('_Contact', 0, GETUTCDATE());
insert into ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('SIH', 0, GETUTCDATE());
insert into ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('TCC', 0, GETUTCDATE());
insert into ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('CAGE', 0, GETUTCDATE());
insert into ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('DAST', 0, GETUTCDATE());
insert into ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('PHQ-9', 0, GETUTCDATE());
insert into ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('HITS', 0, GETUTCDATE());

GO

---- Make Address fields nullable because of screening frequency feature for patient contact screening
ALTER TABLE dbo.ScreeningResult ALTER COLUMN StreetAddress nvarchar(512) NULL;
ALTER TABLE dbo.ScreeningResult ALTER COLUMN City nvarchar(255)	NULL;
ALTER TABLE dbo.ScreeningResult ALTER COLUMN StateID char(2) NULL;
ALTER TABLE dbo.ScreeningResult ALTER COLUMN ZipCode char(5) NULL;
ALTER TABLE dbo.ScreeningResult ALTER COLUMN Phone char(14) NULL;

GO

-- #3 feature - disabling sections
IF NOT EXISTS(SELECT NULL FROM syscolumns where id = OBJECT_ID('ScreeningSectionAge') AND name = 'IsEnabled')
BEGIN
ALTER TABLE dbo.ScreeningSectionAge ADD IsEnabled bit NOT NULL DEFAULT 1;
END

-------------------------------------------------------------------
--- VLADA's changes

--Update Screenig Secion Name
Update dbo.ScreeningSection
SET ScreeningSectionName = 'Alcohol Use (CAGE)' 
Where ScreeningSectionID = 'CAGE'
GO
Update dbo.ScreeningSection
SET ScreeningSectionName = 'Depression (PHQ-9)'
Where ScreeningSectionID = 'PHQ-9'

Update dbo.ScreeningSection
SET ScreeningSectionName = 'Intimate Partner/Domestic Violence (HITS)'
Where ScreeningSectionID = 'HITS'
Go


Update dbo.ScreeningSection
SET ScreeningSectionName = 'Non-Medical Drug Use (DAST-10)'
Where ScreeningSectionID = 'DAST'
Go

--Add new columns to ScreeningScoreLevel

--DELETE COLUMN if was created
IF  EXISTS(SELECT NULL FROM sys.columns c JOIN sys.tables t ON c.object_id = t.object_id 
    Where t.name = 'ScreeningScoreLevel' AND c.name = 'ScoreName')
    BEGIN
 ALTER Table dbo.ScreeningScoreLevel DROP COLUMN ScoreName;
END
GO


IF NOT EXISTS(SELECT NULL FROM sys.columns c JOIN sys.tables t ON c.object_id = t.object_id 
    Where t.name = 'ScreeningScoreLevel' AND c.name = 'Indicates')
    BEGIN
 ALTER Table dbo.ScreeningScoreLevel
 Add Indicates nvarchar(max) NULL;
END
GO


-- CAGE
Update dbo.ScreeningScoreLevel SET 
	Name = 'NEGATIVE'
	,Indicates = 'No problems reported'
Where ScreeningSectionID = 'CAGE' AND ScoreLevel = 0;
Update dbo.ScreeningScoreLevel SET
     Name = 'Evidence of AT RISK'
     ,Indicates = 'Need for further clinical investigation, including questions on amount, frequency, etc.'
Where ScreeningSectionID = 'CAGE' AND ScoreLevel = 1;    
Update dbo.ScreeningScoreLevel SET
     Name = 'Evidence of CURRENT PROBLEM'
     ,Indicates = 'Need for further clinical investigation and/or referral as indicated by clinician''s expertise'
Where ScreeningSectionID = 'CAGE' AND ScoreLevel = 2;   
Update dbo.ScreeningScoreLevel SET
     Name = 'Evidence of DEPENDENCE until ruled out'
     ,Indicates = 'Evaluate, treat, and/or referral as indicated by clinician''s expertise'
Where ScreeningSectionID = 'CAGE' AND ScoreLevel = 3   


--PHQ-9
Update dbo.ScreeningScoreLevel SET
     Name = 'NONE-MINIMAL depression severity'
     ,Indicates = 'No proposed treatment action'
Where ScreeningSectionID = 'PHQ-9' AND ScoreLevel = 0; --None
   
Update dbo.ScreeningScoreLevel SET
     Name = 'NONE-MINIMAL depression severity'
     ,Indicates = 'No proposed treatment action'
Where ScreeningSectionID = 'PHQ-9' AND ScoreLevel = 1; --Minimal
   
Update dbo.ScreeningScoreLevel SET
     Name = 'MILD depression severity'
     ,Indicates = 'Watchful waiting; repeat PHQ-9 at follow-up'
Where ScreeningSectionID = 'PHQ-9' AND ScoreLevel = 2;

   
Update dbo.ScreeningScoreLevel SET
     Name = 'MODERATE depression severity'
     ,Indicates = 'Treatment plan, considering counseling, follow-up and/or pharmacotherapy'
Where ScreeningSectionID = 'PHQ-9' AND ScoreLevel = 3;  

Update dbo.ScreeningScoreLevel SET
     Name = 'MODERATELY SEVERE depression severity'
     ,Indicates = 'Active treatment with pharmacotherapy and/or psychotherapy'
Where ScreeningSectionID = 'PHQ-9' AND ScoreLevel = 4;  

Update dbo.ScreeningScoreLevel SET
     Name = 'SEVERE depression severity'
     ,Indicates = 'Immediate initiation of pharmacotherapy and, if severe impairment or poor response to therapy, expedited referral to a mental health specialist for psychotherapy and/or collaborative management'
Where ScreeningSectionID = 'PHQ-9' AND ScoreLevel = 5;

  
--HITS
Update dbo.ScreeningScoreLevel SET
     Name = 'NEGATIVE'
     ,Indicates = 'Negative'
Where ScreeningSectionID = 'HITS' AND ScoreLevel = 0   

Update dbo.ScreeningScoreLevel SET
     Name = 'POSITIVE'
     ,Indicates = 'Positive'
Where ScreeningSectionID = 'HITS' AND ScoreLevel = 1

--TCC
Update dbo.ScreeningScoreLevel SET
     Name = 'NEGATIVE'
     ,Indicates = 'Negative'
Where ScreeningSectionID = 'TCC' AND ScoreLevel = 0;   

Update dbo.ScreeningScoreLevel SET
     Name = 'POSITIVE'
     ,Indicates = 'Positive'
Where ScreeningSectionID = 'TCC' AND ScoreLevel = 1;

--SIH
Update dbo.ScreeningScoreLevel SET
     Name = 'NEGATIVE'
     ,Indicates = 'Negative'
Where ScreeningSectionID = 'SIH' AND ScoreLevel = 0;   

Update dbo.ScreeningScoreLevel SET
     Name = 'POSITIVE'
     ,Indicates = 'Positive'
Where ScreeningSectionID = 'SIH' AND ScoreLevel = 1;


--DAST-10
Update dbo.ScreeningScoreLevel SET
     Name = 'NEGATIVE'
     ,Indicates = 'No problems reported'
Where ScreeningSectionID = 'DAST' AND ScoreLevel = 0;

Update dbo.ScreeningScoreLevel SET
     Name = 'LOW LEVEL degree of problem related to drug use'
     ,Indicates = 'Monitor and re-assess at a later date'
Where ScreeningSectionID = 'DAST' AND ScoreLevel = 1;

Update dbo.ScreeningScoreLevel SET
     Name = 'MODERATE LEVEL degree of problem related to drug use'
     ,Indicates = 'Further investigation is required'
Where ScreeningSectionID = 'DAST' AND ScoreLevel = 2;

Update dbo.ScreeningScoreLevel SET
     Name = 'SUBSTANTIAL LEVEL degree of problem related to drug use'
     ,Indicates = 'Assessment required'
Where ScreeningSectionID = 'DAST' AND ScoreLevel = 3;
Update dbo.ScreeningScoreLevel SET
     Name = 'SEVERE LEVEL degree of problem related to drug use'
     ,Indicates = 'Assessment required'
Where ScreeningSectionID = 'DAST' AND ScoreLevel = 4;


------------------------------

