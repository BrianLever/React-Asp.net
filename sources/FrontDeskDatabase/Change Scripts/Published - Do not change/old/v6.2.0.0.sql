GO
print 'Populating dbo.NewVisitReferralRecommendation...'
GO
MERGE INTO dbo.NewVisitReferralRecommendation as target
USING (VALUES
(1, 'In-medical', 1),
(2, 'BHS dept.', 2),
(3, 'Internal medical provider', 3),
(4, 'Internal psychiatrist', 3),
(5, 'External BHS provider', 4),
(6, 'External psychiatrist', 5),
(7, ' Other', 6),
(8, 'Not indicated/offered', 100)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;

print 'Populating dbo.[PatientAttendedVisit]...'
GO

MERGE INTO dbo.PatientAttendedVisit as target
USING (VALUES
(1, 'In-medical', 1),
(2, 'BHS dept.', 2),
(3, 'Internal medical provider', 3),
(4, 'Internal psychiatrist', 4),
(5, 'External BHS provider', 5),
(6, 'External psychiatrist', 6),
(7, 'Other', 7),
(8, 'Not indicated/offered', 8),
(9, 'Unknown', 9),
(10, 'No', 10)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

print 'Populating dbo.[TreatmentAction]...'
GO

MERGE INTO dbo.TreatmentAction as target
USING (VALUES
(1, 'Evaluation', 1),
(2, 'Education', 2),
(3, 'Brief Intervention', 3),
(4, 'Brief Treatment', 4),
(5, 'Referral to Treatment', 5),
(7, 'Crisis Intervention', 6),
(8, 'RX by Medical Provider', 7),
(6, 'Other', 8)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

UPDATE dbo.BhsDemographics SET RaceID = 1 WHERE RaceID=2 and EXISTS(SELECT 1 FROM dbo.Race WHERE ID = 8);
UPDATE dbo.BhsDemographics SET RaceID = 7 WHERE RaceID=8;


print 'Populating dbo.[Race]...'
GO
MERGE INTO dbo.[Race] as target
USING (VALUES
(1, 'American Indian or Alaska Native', 2),
(2, 'Asian', 3),
(3, 'Black or African American', 4),
(4, 'Hispanic or Latino', 5),
(5, 'Native Hawaiian or Other Pacific Islander ', 6),
(6, 'Unknown', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

DELETE FROM dbo.Race WHERE ID = 7;
;
GO

print 'Populating dbo.[Gender]...'
GO
MERGE INTO dbo.[Gender] as target
USING (VALUES
(1, 'Male', 2),
(2, 'Female', 3),
(3, 'Transgender', 4),
(4, 'Unknown', 5)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


UPDATE dbo.BhsDemographics SET SexualOrientationID = 4 WHERE SexualOrientationID=5;

print 'Populating dbo.[SexualOrientation]...'
GO
MERGE INTO dbo.[SexualOrientation] as target
USING (VALUES
(1, 'Straight or heterosexual', 2),
(2, 'Bisexual', 3),
(3, 'Lesbian, gay, or homosexual', 4),
(4, 'Unknown', 5)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

DELETE FROM dbo.[SexualOrientation] WHERE ID = 5;

;
GO


print 'Populating dbo.[MaritalStatus]...'
GO
MERGE INTO dbo.[MaritalStatus] as target
USING (VALUES
(1, 'Divorced', 2),
(2, 'Married', 3),
(3, 'Partner', 3),
(4, 'Single', 4),
(5, 'Widowed', 5),
(6, 'Unknown', 6)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


---------------------------------------------
--- UPDATE QUESTIONS
---------------------------------------------

UPDATE dbo.ScreeningSection
SET QuestionText = 'Does anyone in the home smoke tobacco\n(such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?'
WHERE ScreeningSectionID = 'SIH' and ScreeningID = 'BHS'
;


MERGE INTO ScreeningSectionQuestion as Target
USING( VALUES
('SIH', 1, NULL, 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 1, 1, 10),
('TCC', 2, NULL, 'Do you smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 1, 0, 100)
) as Source(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
	ON Target.ScreeningSectionID = Source.ScreeningSectionID AND Target.QuestionID = Source.QuestionID

WHEN MATCHED THEN
	UPDATE SET 
		PreambleText = Source.PreambleText, 
		QuestionText = Source.QuestionText,
		AnswerScaleID = Source.AnswerScaleID,
		IsMainQuestion = Source.IsMainQuestion,
		OrderIndex = Source.OrderIndex

;
GO


IF EXISTS(SELECT * 
FROM sys.indexes 
WHERE name='IX__ScreeningSectionQuestionResult_Answer' AND object_id = OBJECT_ID('dbo.ScreeningSectionQuestionResult'))
SET NOEXEC ON
GO

CREATE NONCLUSTERED INDEX IX__ScreeningSectionQuestionResult_Answer
ON [dbo].[ScreeningSectionQuestionResult] ([ScreeningSectionID],[QuestionID],[AnswerValue])
INCLUDE ([ScreeningResultID])
GO

SET NOEXEC OFF
GO



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.2.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.2.0.0');