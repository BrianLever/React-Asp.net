--- Permissions


CREATE ROLE [screendox_appaccount]
    AUTHORIZATION [dbo];
GO

GRANT SELECT, EXECUTE ON SCHEMA :: dbo TO [screendox_appaccount]
ALTER AUTHORIZATION ON SCHEMA::[HangFire] TO screendox_appaccount
GRANT SELECT, EXECUTE ON SCHEMA::[HangFire]  TO [screendox_appaccount];
GRANT SELECT, EXECUTE ON SCHEMA::[export]  TO [screendox_appaccount];



GRANT DELETE, UPDATE, INSERT ON dbo.Users TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.UsersInRoles TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.UserDetails TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.UserPasswordHistory TO [screendox_appaccount];

GRANT UPDATE ON dbo.SystemSettings TO [screendox_appaccount];


GRANT INSERT, UPDATE, DELETE ON dbo.BranchLocation TO [screendox_appaccount]
GRANT INSERT, UPDATE, DELETE ON dbo.Kiosk TO [screendox_appaccount]
GRANT INSERT, UPDATE, DELETE ON dbo.Users_BranchLocation TO [screendox_appaccount]

GRANT UPDATE, DELETE, INSERT ON dbo.ScreeningSection TO [screendox_appaccount]

GRANT UPDATE, INSERT, DELETE ON dbo.ScreeningResult TO [screendox_appaccount]

GRANT INSERT, DELETE ON dbo.ScreeningSectionResult TO [screendox_appaccount]
GRANT INSERT, DELETE ON dbo.ScreeningSectionQuestionResult TO [screendox_appaccount]


---
GRANT INSERT,DELETE,UPDATE ON ErrorLog TO [screendox_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.License TO [screendox_appaccount]
GRANT INSERT, UPDATE ON dbo.ScreeningSection TO [screendox_appaccount]
GRANT INSERT, UPDATE ON dbo.ScreeningSectionAge TO [screendox_appaccount]
GRANT INSERT, UPDATE ON dbo.ScreeningFrequency TO [screendox_appaccount]

GRANT INSERT,DELETE,UPDATE ON dbo.SecurityEvent TO [screendox_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.SecurityLog TO [screendox_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.SystemSettings TO [screendox_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.UserPasswordHistory TO [screendox_appaccount]

GRANT INSERT,DELETE,UPDATE ON dbo.License TO [screendox_appaccount]


GRANT INSERT,DELETE,UPDATE ON dbo.RpmsCredentials TO [screendox_appaccount]


GRANT INSERT,DELETE,UPDATE ON dbo.BhsVisit TO [screendox_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.BhsDemographics TO [screendox_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.VisitSettings TO [screendox_appaccount]


GRANT INSERT,DELETE,UPDATE ON dbo.BhsFollowUp TO [screendox_appaccount]

GRANT INSERT,DELETE,UPDATE ON dbo.ScreeningTimeLog TO [screendox_appaccount]


GRANT SELECT, INSERT, DELETE, UPDATE ON [export].SmartExportLog  TO screendox_appaccount;
GRANT SELECT, INSERT ON [export].[PatientNameCorrectionLog]  TO screendox_appaccount;

GRANT DELETE, UPDATE, INSERT ON dbo.ScreeningProfile TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ScreeningProfileFrequency TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ScreeningProfileSectionAge TO [screendox_appaccount];


-- columbia
GRANT UPDATE, INSERT ON dbo.ColumbiaSuicideReport TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaIntensityIdeation TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicidalIdeation TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicideBehaviorAct TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicideRiskAssessmentReport TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors TO [screendox_appaccount];

GO