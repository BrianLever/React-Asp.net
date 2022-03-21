--- Permissions

GRANT SELECT, EXECUTE ON SCHEMA :: dbo TO [frontdesk_appaccount]
ALTER AUTHORIZATION ON SCHEMA::[HangFire] TO frontdesk_appaccount
GRANT EXECUTE ON SCHEMA::[HangFire]  TO [frontdesk_appaccount];
GRANT EXECUTE ON SCHEMA::[export]  TO [frontdesk_appaccount];


GRANT DELETE, UPDATE, INSERT ON dbo.Users TO [frontdesk_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.UsersInRoles TO [frontdesk_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.UserDetails TO [frontdesk_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.UserPasswordHistory TO [frontdesk_appaccount];

GRANT UPDATE ON dbo.SystemSettings TO [frontdesk_appaccount];


GRANT INSERT, UPDATE, DELETE ON dbo.BranchLocation TO [frontdesk_appaccount]
GRANT INSERT, UPDATE, DELETE ON dbo.Kiosk TO [frontdesk_appaccount]
GRANT INSERT, UPDATE, DELETE ON dbo.Users_BranchLocation TO [frontdesk_appaccount]

GRANT UPDATE, DELETE, INSERT ON dbo.ScreeningSection TO [frontdesk_appaccount]

GRANT UPDATE, DELETE ON dbo.ScreeningResult TO [frontdesk_appaccount]
GRANT DELETE ON dbo.ScreeningSectionResult TO [frontdesk_appaccount]
GRANT DELETE ON dbo.ScreeningSectionQuestionResult TO [frontdesk_appaccount]


---
GRANT INSERT,DELETE,UPDATE ON ErrorLog TO [frontdesk_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.License TO [frontdesk_appaccount]
GRANT INSERT, UPDATE ON dbo.ScreeningSection TO [frontdesk_appaccount]
GRANT INSERT, UPDATE ON dbo.ScreeningSectionAge TO [frontdesk_appaccount]
GRANT INSERT, UPDATE ON dbo.ScreeningFrequency TO [frontdesk_appaccount]

GRANT INSERT,DELETE,UPDATE ON dbo.SecurityEvent TO [frontdesk_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.SecurityLog TO [frontdesk_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.SystemSettings TO [frontdesk_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.UserPasswordHistory TO [frontdesk_appaccount]

GRANT INSERT,DELETE,UPDATE ON dbo.License TO [frontdesk_appaccount]


GRANT INSERT,DELETE,UPDATE ON dbo.BhsVisit TO [frontdesk_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.BhsDemographics TO [frontdesk_appaccount]
GRANT INSERT,DELETE,UPDATE ON dbo.VisitSettings TO [frontdesk_appaccount]


GRANT INSERT,DELETE,UPDATE ON dbo.BhsFollowUp TO [frontdesk_appaccount]

GRANT INSERT,DELETE,UPDATE ON dbo.ScreeningTimeLog TO [frontdesk_appaccount]


GRANT INSERT, DELETE, UPDATE ON [export].SmartExportLog  TO frontdesk_appaccount;
GRANT INSERT ON [export].[PatientNameCorrectionLog]  TO frontdesk_appaccount;

GRANT DELETE, UPDATE, INSERT ON dbo.ScreeningProfile TO [frontdesk_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ScreeningProfileFrequency TO [frontdesk_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ScreeningProfileSectionAge TO [frontdesk_appaccount];

GO