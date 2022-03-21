CREATE LOGIN [smartexport_appuser] WITH PASSWORD = 'Q1w2e3r4t5-34614!.' , CHECK_EXPIRATION = OFF;
GO
CREATE USER [smartexport_appuser] FOR LOGIN smartexport_appuser WITH DEFAULT_SCHEMA = dbo ;

GO


ALTER AUTHORIZATION ON SCHEMA::[HangFire] TO [smartexport_appuser]
GO

GRANT CREATE TABLE TO [smartexport_appuser]
GO