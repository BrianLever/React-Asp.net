print 'Populating dbo.NewVisitReferralRecommendation...'
GO
MERGE INTO dbo.NewVisitReferralRecommendation as target
USING (VALUES
(1, 'Behavioral Health Department', 1),
(2, 'Crisis/Emergency Service', 2),
(3, 'Medical Department', 3),
(4, 'Medication	Assisted Treatment (MAT)', 4),
(5, 'Pain Management', 5),
(6, 'Psychiatrist', 6),
(9, 'RX', 7),
(7, 'Other', 8),
(8, 'Not indicated/offered', 100)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;

GO
;

---------------------------------
GO
print 'Populating dbo.[NewVisitReferralRecommendationAccepted]...'
GO
MERGE INTO dbo.[NewVisitReferralRecommendationAccepted] as target
USING (VALUES
(1, 'Yes', 1),
(2, 'No', 2),
(8, 'Not indicated/offered', 100)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

---------------------------------
GO
print 'Populating dbo.[ReasonNewVisitReferralRecommendationNotAccepted]...'
GO
MERGE INTO dbo.[ReasonNewVisitReferralRecommendationNotAccepted] as target
USING (VALUES
(0, 'Accepted', 1),
(1, 'Service perceived as not needed', 3),
(2, 'Has existing provider', 4),
(3, 'Wants other (external) provider', 5),
(4, 'Distance – too far away', 6),
(5, 'Concerned about confidentiality', 7),
(6, 'No transportation', 8),
(7, 'Work', 9),
(8, 'Not indicated/offered', 2),
(9, 'No childcare', 10),
(10, 'Other responsibility', 11),
(11, 'Too ill, elderly, or handicap', 12),
(12, 'Decline to answer', 13)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;

---------------------------------
GO
print 'Populating dbo.[Discharged]...'
GO
MERGE INTO dbo.[Discharged] as target
USING (VALUES
(0, 'No', 1),
(1, 'Service completed', 2),
(2, 'Symptom reduction', 3),
(3, 'Patient requested discontinuation of service', 3),
(4, 'Address changed – out of service area', 4),
(5, 'Could not contact', 5),
(6, 'Transferred to different provider', 6),
(7, 'Deceased', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[Race]...'
GO
MERGE INTO dbo.[Race] as target
USING (VALUES
(1, 'American Indian', 2),
(7, 'Alaska Native', 3),
(2, 'Asian', 4),
(3, 'Black or African American', 5),
(4, 'Hispanic or Latino', 6),
(5, 'Native Hawaiian or Other Pacific Islander', 7),
(8, 'White', 8),
(9, 'Other', 9),
(6, 'Unknown', 10)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

print 'Populating dbo.[Gender]...'
GO
MERGE INTO dbo.[Gender] as target
USING (VALUES
(1, 'Male', 2),
(2, 'Female', 3),
(3, 'Transgender', 4),
(5, 'Not Female, Male, or Transgender', 5),
(4, 'Don’t Know', 6),
(6, 'Decline to Answer', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[SexualOrientation]...'
GO
MERGE INTO dbo.[SexualOrientation] as target
USING (VALUES
(1, 'Straight/Heterosexual', 2),
(3, 'Gay/Lesbian', 3),
(2, 'Bisexual', 4),
(5, 'Other', 5),
(4, 'Don’t Know', 6),
(6, 'Decline to Answer', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

print 'Populating dbo.[MaritalStatus]...'
GO
MERGE INTO dbo.[MaritalStatus] as target
USING (VALUES
(4, 'Single', 2),
(2, 'Married', 3),
(3, 'Partner', 4),
(6, 'Separated', 5),
(1, 'Divorced', 6),
(5, 'Widowed', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

print 'Populating dbo.[EducationLevel]...'
GO

MERGE INTO dbo.[EducationLevel] as target
USING (VALUES
(1, 'Elementary School', 2),
(2, 'Some High School', 3),
(11, 'Completed GED or High School Equivalent', 4),
(3, 'Completed High School Diploma', 5),

(5, 'Technical School', 6),
(6, 'AA degree', 7),
(7, 'Bachelor''s Degree', 8),
(8, 'Master''s Degree', 9),
(10, 'Doctoral Degree', 10)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE ;
GO



print 'Populating dbo.[LivingOnReservation]...'
GO

MERGE INTO dbo.LivingOnReservation as target
USING (VALUES
(1, 'On reservation', 2),
(2, 'Off reservation', 3)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[MilitaryExperience]...'
GO

MERGE INTO dbo.MilitaryExperience as target
USING (VALUES
(1, 'None', 2),
(2, 'Active duty', 3),
(3, 'Veteran', 4),
(4, 'Deployed to a combat zone', 5)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex, LastModifiedDateUTC = GETUTCDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
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
(6, 'Other', 8)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;
GO

print 'Populating dbo.[PatientAttendedVisit]...'
GO

MERGE INTO dbo.PatientAttendedVisit as target
USING (VALUES
(1, 'Yes', 1),
(2, 'No', 2),
(3, 'Unknown', 3),
(4, 'Not indicated/offered', 4)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;
GO

print 'Populating dbo.[FollowUpContactOutcome]...'
GO
MERGE INTO dbo.FollowUpContactOutcome as target
USING (VALUES
(0, 'None', 1),
(1, 'Talked with patient or parent', 2),
(2, 'Left message', 3),
(3, 'Phone not working', 4),
(4, 'Unable to leave message', 5)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO



print 'Populating dbo.[DrugOfChoice]...'
GO

MERGE INTO dbo.DrugOfChoice as target
USING (VALUES
(0, '(None) Don’t Use Any Other Drugs', 1),
(1, 'Marijuana/Cannabis/Wax/Hashish', 2),
(2, 'Methamphetamine', 3),
(3, 'Other Amphetamines', 4),
(4, 'Benzodiazepines', 5),
(5, 'Opioid/Heroin', 6),
(6, 'Opioid/Medication', 7),
(7, 'Cocaine/Crack', 8),
(8, 'Hallucinogens/Psychedelics', 9),
(9, 'Sedatives/Hypnotics/Non-Benzo Tranquilizers', 10),
(10, 'Inhalants', 11),
(11, 'Barbiturates/Downers', 12),
(12, 'PCP/Ketamine/GHB/Designer Drugs', 13),
(13, 'Other Stimulants', 14),
(14, 'Other', 15)

) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;
GO



print 'Populating dbo.[SecurityEventCategory]...'
GO
MERGE INTO dbo.SecurityEventCategory as target
USING (VALUES(1, 'System Security'),
                    (2, 'Accessing patient info'),
                    (3, 'Branch management'),
                    (4, 'Kiosk management')
) as source(SecurityEventCategoryID, CategoryName) 
    ON target.SecurityEventCategoryID = source.SecurityEventCategoryID
WHEN MATCHED THEN 
    UPDATE SET CategoryName = source.CategoryName
WHEN NOT MATCHED BY TARGET THEN
    INSERT(SecurityEventCategoryID, CategoryName) 
        VALUES (source.SecurityEventCategoryID, source.CategoryName)
;
GO

GO	
print 'Populating dbo.[SecurityEvent]...'
GO

MERGE INTO dbo.SecurityEvent as target
USING ( VALUES
--system security
(1, 1, 'User was logged into the system',1),
(2, 1, 'Password was changed',1),
(3, 1, 'Security question and/or answer were changed',1),
(4, 1, 'New user was created',1),
(5, 1, 'New account was activated',1),
-- accessing screen results 
(6, 2, 'Behavioral Health Screening Report was read',1),
(7, 2, 'Behavioral Health Screening Report was printed',1),
(12, 2, 'Behavioral Health Screening Report was removed',1),
(13, 2, 'Patient contact information was changed',1),	  
(14, 2, 'Behavioral Health Screening Report was exported',1),
(15, 2, 'BHS Visit Information was completed',1),
(16, 2, 'BHS Visit was created manually',1),
(18, 2, 'BHS Patient Demographics was completed',1),
(19, 2, 'BHS Follow-Up was completed',1),
(20, 2, 'Patient address was updated from EHR',1),
(21, 2, 'BHS Visit Report was printed',1),
(22, 2, 'BHS Follow-Up Report was printed',1),
(23, 2, 'BHS Patient Demographics was printed',1),
(24, 2, 'Patient Demographics was printed',1),
(25, 2, 'Drug List Use was changed',1),


-- Branch location mgmt
(8, 3, 'New branch location was created',1),
(9, 3, 'Branch location was removed',1),
-- Kiosk mgmt
(10, 4, 'New kiosk was registered',1),
(11, 4, 'Kiosk was removed',1)
) as source(SecurityEventID, SecurityEventCategoryID, [Description], Enabled) 
    ON target.SecurityEventID = source.SecurityEventID
WHEN MATCHED THEN 
    UPDATE SET [Description] = source.[Description], Enabled = source.Enabled
WHEN NOT MATCHED BY TARGET THEN
    INSERT(SecurityEventID, SecurityEventCategoryID, [Description], Enabled) 
        VALUES (source.SecurityEventID, source.SecurityEventCategoryID, source.[Description], Enabled)
;
GO
