 --alter table BranchLocation
--	alter column [Name] nvarchar(128) NOT NULL

-- Create Kiosk table --
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Kiosk')
	BEGIN
		DROP  Table dbo.Kiosk
	END
GO

CREATE TABLE dbo.Kiosk
(
	KioskID smallint identity(1000,1) NOT NULL,
	KioskName nvarchar(255) NOT NULL,
	Description nvarchar(max) NULL,
	CreatedDate dateTimeoffset NOT NULL,
	LastActivityDate dateTimeoffset NULL,
	BranchLocationID int NOT NULL,
	CONSTRAINT PK_Kiosk PRIMARY KEY (KioskID),
	CONSTRAINT FK_Kiosk_BranchLocation FOREIGN KEY (BranchLocationID) 
		REFERENCES BranchLocation(BranchLocationID) ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT [UQ__Kiosk] UNIQUE ([KioskName] ASC)	
)
GO

--



----- CHANGE KioskID data type ---------------------
DROP INDEX IX_ScreeningResult_IsDeleted ON ScreeningResult;
ALTER TABLE dbo.ScreeningResult DROP  COLUMN KioskID
ALTER TABLE dbo.ScreeningResult ADD KioskID smallint NULL;
CREATE INDEX IX_ScreeningResult_IsDeleted ON dbo.ScreeningResult(IsDeleted) INCLUDE(FirstName, LastName, MiddleName, Birthday, CreatedDate, ScreeningID, KioskID )
Where IsDeleted = 0
GO
ALTER TABLE dbo.ScreeningResult
ADD CONSTRAINT FK_ScreeningResult__Kiosk FOREIGN KEY(KioskID) 
		REFERENCES dbo.Kiosk(KioskID) 
		ON DELETE SET NULL ON UPDATE CASCADE
GO
-----------------------------------------------------

----- CHANGE KioskID data type ---------------------
DROP INDEX IX_ErrorLog_CreatedDate ON ErrorLog;
ALTER TABLE dbo.ErrorLog DROP  COLUMN KioskID
ALTER TABLE dbo.ErrorLog ADD KioskID smallint NULL;
CREATE INDEX IX_ErrorLog_CreatedDate ON dbo.ErrorLog(CreatedDate DESC) INCLUDE(KioskID,ErrorMessage);
GO
-----------------------------------------------------








---------- CHANGE QUESTIONS ------------------------------

UPDATE ScreeningSection 
SET QuestionText = 'Do you feel UNSAFE in your home?' 
WHERE ScreeningSectionID = 'HITS';

GO


UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Do you want help quitting?'
WHERE ScreeningSectionID = 'TCC' AND QuestionID = 1;


----
UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Have you ever felt you should CUT down on your drinking?'
WHERE ScreeningSectionID = 'CAGE' AND QuestionID = 1;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Have people ANNOYED you by criticizing your drinking?'
WHERE ScreeningSectionID = 'CAGE' AND QuestionID = 2;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Have you ever felt bad or GUILTY about your drinking?'
WHERE ScreeningSectionID = 'CAGE' AND QuestionID = 3;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Have you ever had a drink first thing in the morning to steady your nerves or get rid of a hangover (EYE-OPENER)?'
WHERE ScreeningSectionID = 'CAGE' AND QuestionID = 4;

-------------------------------

UPDATE ScreeningSectionQuestion
SET PreambleText = 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems?' 
WHERE ScreeningSectionID = 'PHQ-9' AND PreambleText IS NOT NULL;


UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Little interest or pleasure in doing things'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 1;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Feeling down, depressed, or hopeless'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 2;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Trouble falling or staying asleep, or sleeping too much'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 3;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Feeling tired or having little energy'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 4;


UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Poor appetite or overeating'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 5;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Feeling bad about yourself - or that you are a failure or have let yourself or your family down'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 6;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Trouble concentrating on things, such as reading the newspaper or watching television'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 7;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Moving or speaking so slowly that other people could have noticed. Or the opposite - being so fidgety or restless that you have been moving around a lot more than usual'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 8;


UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Thoughts that you would be better off dead or of hurting yourself in some way'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 9;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?'
WHERE ScreeningSectionID = 'PHQ-9' AND QuestionID = 10;

-------
UPDATE ScreeningSectionQuestion
SET PreambleText = 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:'
WHERE ScreeningSectionID = 'HITS' AND PreambleText IS NOT NULL;


UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'Physically HURT you?'
WHERE ScreeningSectionID = 'HITS' AND QuestionID = 1;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'INSULT or talk down to you?'
WHERE ScreeningSectionID = 'HITS' AND QuestionID = 2;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'THREATEN you with physical harm?'
WHERE ScreeningSectionID = 'HITS' AND QuestionID = 3;

UPDATE ScreeningSectionQuestion
SET 
QuestionText = 'SCREAM or curse at you?'
WHERE ScreeningSectionID = 'HITS' AND QuestionID = 4;





---------------------------------------------------------------------


IF EXISTS(SELECT * FROM sys.objects where type='FN' and name='fn_GetPatientName')
	DROP FUNCTION fn_GetPatientName;
GO

CREATE FUNCTION [dbo].[fn_GetPatientName](
	@LastName nvarchar(255), 
	@FirstName nvarchar(255), 
	@MiddleName nvarchar(255)
)
RETURNS nvarchar(max)
WITH SCHEMABINDING
AS
BEGIN

DECLARE @comma bit = 0; -- where comma was added
DECLARE @Result nvarchar(max);
SET @Result = ISNULL(@LastName, '');

IF LEN(ISNULL(@FirstName, '')) > 0
BEGIN
	IF LEN(@Result) > 0
	BEGIN
		SET @Result = @Result + ', ';
		SET @comma = 1;
		
		SET @Result = @Result + @LastName;
	END
	
END	

IF LEN(ISNULL(@MiddleName, '')) > 0
BEGIN
	IF LEN(@Result) > 0
	BEGIN
		IF @comma = 1
			SET @Result = @Result + ' ';
		ELSE 
		SET @Result = @Result + ', ';
		
		SET @Result = @Result + @MiddleName;
	END

	
END

RETURN @Result;
END
GO

GRANT EXECUTE ON dbo.[fn_GetPatientName] TO frontdesk_appuser; 
-------------------------

-------------- Security Log ------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityEventCategory]') AND type in (N'U'))
DROP TABLE [dbo].[SecurityEventCategory]

GO


CREATE TABLE [dbo].[SecurityEventCategory](
	[SecurityEventCategoryID] [int] NOT NULL,
	[CategoryName] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_SecurityEventCategory] PRIMARY KEY CLUSTERED 
(
	[SecurityEventCategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO SecurityEventCategory(SecurityEventCategoryID, CategoryName)
		VALUES	(1, 'System Security'),
				(2, 'Accessing patient info'),
				(3, 'Branch management'),
				(4, 'Kiosk management')

GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SecurityEvent_SecurityEventCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[SecurityEvent]'))
ALTER TABLE [dbo].[SecurityEvent] DROP CONSTRAINT [FK_SecurityEvent_SecurityEventCategory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityEvent]') AND type in (N'U'))
DROP TABLE [dbo].[SecurityEvent]
GO

CREATE TABLE [dbo].[SecurityEvent](
	[SecurityEventID] [int] NOT NULL,
	[SecurityEventCategoryID] [int] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_SecurityLogAction] PRIMARY KEY CLUSTERED 
(
	[SecurityEventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SecurityEvent]  WITH CHECK ADD  CONSTRAINT [FK_SecurityEvent_SecurityEventCategory] FOREIGN KEY([SecurityEventCategoryID])
REFERENCES [dbo].[SecurityEventCategory] ([SecurityEventCategoryID])
GO

ALTER TABLE [dbo].[SecurityEvent] CHECK CONSTRAINT [FK_SecurityEvent_SecurityEventCategory]
GO


insert into SecurityEvent(SecurityEventID, SecurityEventCategoryID, [Description])
				--system security
			values(1, 1, 'User logged into the system'),
				  (2, 1, 'Password was changed'),
				  (3, 1, 'Security question and/or answer were changed'),
				  (4, 1, 'New user was created'),
				  (5, 1, 'New account was activated'),
				-- accessing screen results 
				  (6, 2, 'Behavioral health screening report was viewed'),
				  (7, 2, 'Behavioral health screening report was printed'),
				-- Branch location mgmt
				  (8, 3, 'New branch location was created'),
				  (9, 3, 'Branch location was removed.'),
				-- Kiosk mgmt
				  (10, 3, 'New kiosk was registered'),
				  (11, 3, 'Kiosk was removed.');
				  
GO
---------------------------------------


---- Add column to ScreeningResult ---
ALTER TABLE dbo.ScreeningResult
	ADD WithErrors bit NOT NULL CONSTRAINT DF_ScreeningResult_WithErrors DEFAULT(0);
GO	