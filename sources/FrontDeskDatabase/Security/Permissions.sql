--- Permissions
GRANT EXECUTE ON dbo.[fn_GetFullName] TO frontdesk_appuser;
GRANT EXECUTE ON dbo.[fn_GetPatientName] TO frontdesk_appuser;



GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.Users TO frontdesk_appuser;
GRANT SELECT ON dbo.Roles TO frontdesk_appuser;
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UsersInRoles TO frontdesk_appuser;
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UserDetails TO frontdesk_appuser;
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UserPasswordHistory TO frontdesk_appuser;
GRANT SELECT ON dbo.SecurityQuestion TO frontdesk_appuser;


GRANT SELECT,UPDATE ON dbo.SystemSettings TO frontdesk_appuser;


GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.BranchLocation TO frontdesk_appuser
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Kiosk TO frontdesk_appuser
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Users_BranchLocation TO frontdesk_appuser
GRANT SELECT ON dbo.State TO frontdesk_appuser


GRANT SELECT ON dbo.AnswerScale TO frontdesk_appuser
GRANT SELECT ON dbo.AnswerScaleOption TO frontdesk_appuser
GRANT SELECT ON dbo.Screening TO frontdesk_appuser
GRANT SELECT ON dbo.ScreeningSection TO frontdesk_appuser
GRANT SELECT ON dbo.ScreeningSectionQuestion TO frontdesk_appuser
GRANT SELECT, UPDATE, DELETE, INSERT ON dbo.ScreeningSection TO frontdesk_appuser

GRANT SELECT ON dbo.ScreeningScoreLevel TO frontdesk_appuser

GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.ScreeningResult TO frontdesk_appuser
GRANT SELECT, INSERT, DELETE ON dbo.ScreeningSectionResult TO frontdesk_appuser
GRANT SELECT, INSERT, DELETE ON dbo.ScreeningSectionQuestionResult TO frontdesk_appuser


---
GRANT SELECT,INSERT,DELETE,UPDATE ON ErrorLog TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.License TO frontdesk_appuser
GRANT SELECT,INSERT, UPDATE ON dbo.ScreeningSection TO frontdesk_appuser
GRANT SELECT,INSERT, UPDATE ON dbo.ScreeningSectionAge TO frontdesk_appuser
GRANT SELECT,INSERT, UPDATE ON dbo.ScreeningFrequency TO frontdesk_appuser

GRANT SELECT ON dbo.SecurityEventCategory TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SecurityEvent TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SecurityLog TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SystemSettings TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.UserPasswordHistory TO frontdesk_appuser

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.License TO frontdesk_appuser


GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsVisit TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsDemographics TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.VisitSettings TO frontdesk_appuser


GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsFollowUp TO frontdesk_appuser

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.ScreeningTimeLog TO frontdesk_appuser

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.ScreeningProfile TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.ScreeningProfileFrequency TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.ScreeningProfileSectionAge TO frontdesk_appuser


GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.LookupValuesDeleteLog TO frontdesk_appuser

GRANT SELECT, EXECUTE ON SCHEMA :: dbo TO frontdesk_appuser

ALTER AUTHORIZATION ON SCHEMA::[HangFire] TO [smartexport_appuser]


GRANT CREATE TABLE TO [smartexport_appuser]

GRANT EXECUTE ON SCHEMA::[dbo]  TO frontdesk_appuser;

GRANT EXECUTE ON SCHEMA::[HangFire]  TO frontdesk_appuser;

GRANT EXECUTE ON SCHEMA::[export]  TO frontdesk_appuser;

GRANT EXECUTE ON SCHEMA::[export]  TO [smartexport_appuser];


GRANT EXECUTE ON [dbo].[uspCreateNewScreeningProfile] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspUpdateScreeningProfileAgeSettings] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspUpdateScreeningProfileFrequency] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspGetAllBranchLocations] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspGetModifiedSectionMinimalAgeSettingsForKiosk] TO frontdesk_appuser
GRANT EXECUTE ON [dbo].[uspGetScreeningProfileByKioskID] TO frontdesk_appuser
GRANT EXECUTE ON [dbo].[uspGetModifiedSectionMinimalAgeSettings] TO frontdesk_appuser
GRANT EXECUTE ON dbo.ScreeningSectionQuestionResult TO frontdesk_appuser
GRANT DELETE ON [export].SmartExportLog  TO frontdesk_appuser;
GRANT SELECT ON [export].[PatientNameMap]  TO frontdesk_appuser;

GRANT INSERT,SELECT ON [export].[PatientNameCorrectionLog]  TO frontdesk_appuser;

GO
