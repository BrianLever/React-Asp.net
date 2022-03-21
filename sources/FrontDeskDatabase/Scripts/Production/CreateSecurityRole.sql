CREATE ROLE frontdesk_appaccount

GO
--ALTER ROLE frontdesk_appaccount ADD MEMBER frontdesk_appaccount
GRANT SELECT ON DbVersion TO PUBLIC;
GO
GRANT EXECUTE ON dbo.[fn_GetFullName] TO frontdesk_appaccount;
GO
GRANT EXECUTE ON dbo.[fn_GetPatientName] TO frontdesk_appaccount
GO
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.Users TO frontdesk_appaccount
GO
GRANT SELECT ON dbo.Roles TO frontdesk_appaccount;
GO
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UsersInRoles TO frontdesk_appaccount;
GO
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UserDetails TO frontdesk_appaccount;
GO
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UserPasswordHistory TO frontdesk_appaccount;
GO
GRANT SELECT ON dbo.SecurityQuestion TO frontdesk_appaccount;
GO
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.BranchLocation TO frontdesk_appaccount
GO
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Kiosk TO frontdesk_appaccount
GO
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Users_BranchLocation TO frontdesk_appaccount
GO
GRANT SELECT ON dbo.State TO frontdesk_appaccount
GO
GRANT SELECT ON dbo.AnswerScale TO frontdesk_appaccount
GO
GRANT SELECT ON dbo.AnswerScaleOption TO frontdesk_appaccount
GO
GRANT SELECT ON dbo.Screening TO frontdesk_appaccount
GO
GRANT SELECT ON dbo.ScreeningSectionQuestion TO frontdesk_appaccount
GO
GRANT SELECT, UPDATE, DELETE, INSERT ON dbo.ScreeningSection TO frontdesk_appaccount
GO
GRANT SELECT ON dbo.ScreeningScoreLevel TO frontdesk_appaccount
GO
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.ScreeningResult TO frontdesk_appaccount
GO
GRANT SELECT, INSERT, DELETE ON dbo.ScreeningSectionResult TO frontdesk_appaccount
GO
GRANT SELECT, INSERT, DELETE ON dbo.ScreeningSectionQuestionResult TO frontdesk_appaccount
GO
GRANT SELECT,INSERT,DELETE,UPDATE ON ErrorLog TO frontdesk_appaccount
GO
GRANT SELECT,INSERT, UPDATE ON dbo.ScreeningSectionAge TO frontdesk_appaccount
GO
GRANT SELECT,INSERT, UPDATE ON dbo.ScreeningFrequency TO frontdesk_appaccount
GO
GRANT SELECT ON dbo.SecurityEventCategory TO frontdesk_appaccount
GO
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SecurityEvent TO frontdesk_appaccount
GO
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SecurityLog TO frontdesk_appaccount
GO
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SystemSettings TO frontdesk_appaccount
GO

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.License TO frontdesk_appaccount
GO
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.RpmsCredentials TO frontdesk_appaccount
GO

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsVisit TO frontdesk_appaccount
GO
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsDemographics TO frontdesk_appaccount
GO
GRANT EXECUTE ON SCHEMA::[dbo]  TO frontdesk_appaccount;
GO
GRANT EXECUTE ON SCHEMA::[HangFire]  TO frontdesk_appaccount;
GO
GRANT SELECT, EXECUTE ON SCHEMA::[export]  TO frontdesk_appaccount;
GO
GRANT EXECUTE ON [dbo].[uspCreateNewScreeningProfile] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspUpdateScreeningProfileAgeSettings] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspUpdateScreeningProfileFrequency] TO frontdesk_appuser
GO
GRANT EXECUTE ON [dbo].[uspGetAllBranchLocations] TO frontdesk_appuser
GO