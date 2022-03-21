
----------------------------------------------------------------------------
-------------------------- Set email as nullable --------------------------

alter table [dbo].[Users] 
	alter column Email nvarchar(255) null
----------------------------------------------------------------------------





alter table SecurityEvent add 
	[Enabled] bit not null CONSTRAINT DF_SecurityEvent_Enabled DEFAULT(0)
	
	
delete from SecurityLog

delete from SecurityEvent

insert into SecurityEvent(SecurityEventID, SecurityEventCategoryID, [Description])
				--system security
			values(1, 1, 'User was logged into the system'),
				  (2, 1, 'Password was changed'),
				  (3, 1, 'Security question and/or answer were changed'),
				  (4, 1, 'New user was created'),
				  (5, 1, 'New account was activated'),
				-- accessing screen results 
				  (6, 2, 'Behavioral Health Screening Report was read'),
				  (7, 2, 'Behavioral Health Screening Report was printed'),
				-- Branch location mgmt
				  (8, 3, 'New branch location was created'),
				  (9, 3, 'Branch location was removed'),
				-- Kiosk mgmt
				  (10, 4, 'New kiosk was registered'),
				  (11, 4, 'Kiosk was removed'),
				  (12, 2, 'Behavioral Health Screening Report was removed');



--------------------------------- add branch id relation to security log	
	
alter table [dbo].[SecurityLog] add RelatedBranchID int null

go

alter table [dbo].[SecurityLog] WITH CHECK add constraint [FK_SecurityLog_BranchLocationID] foreign key ([RelatedBranchID])
references [dbo].[BranchLocation]([BranchLocationID])
ON DELETE SET NULL
ON UPDATE CASCADE

go

alter table [dbo].[SecurityLog] check constraint [FK_SecurityLog_BranchLocationID]

--------------------------------------------------------------------------


	
-----------------------------------------------------
---- Lisence ---------------------------------------
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'License')
	BEGIN
		DROP  Table License
	END
GO

CREATE TABLE dbo.License
(
	LicenseKey varchar(128) NOT NULL,
	ActivationKey varchar(128) NULL,
	ActivationRequest varchar(128) NULL,
	CreatedDate datetimeoffset NOT NULL,
	ActivationRequestDate datetimeoffset NULL,
	ActivatedDate datetimeoffset NULL,
	CONSTRAINT PK_License PRIMARY KEY (LicenseKey)
)
GO



GRANT SELECT, UPDATE, DELETE, INSERT ON dbo.License TO frontdesk_appuser 
------------------------------------------------------------


INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('ActivationSupportEmail', 'skryshtop@3si2.com', 'License Activation Email Address','Email address where activation request code need to be send.', NULL, 0);
GO
INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('ActivationSupportEmailSubject', 'FrontDesk License Activation', 'Activation Email Subject Text','Activation Email Subject Text.', NULL, 0);
GO

INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('ActivationRequestEmailTemplate', 'Please activate my FrontDesk Behavioral Health Screener product license.%0A%0AMy activation request code: {0}', 'Activation Request Email Template','Email text for sending product activation request code', NULL, 0);
GO


INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('ExportedSecurityReportMaximumLength', '3000', 'Maximum length of the exported security report', 'The Maximum number of security report''s rows that can be exported to excel document', NULL, 1);

--INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
--VALUES('LicenseExpirationNotificationDays', '90,60,55,45,40,35,30,25,20,15,10,5,4,3,2,1', 'Days of License Expiration Notification', 'The number of days till expiraton when notification must be displayed', NULL, 1);
ALTER TABLE dbo.Kiosk
	ADD Disabled bit NOT NULL CONSTRAINT DF_Kiosk_Disabled DEFAULT(0) WITH VALUES;
GO


ALTER TABLE dbo.BranchLocation
	ADD Disabled bit NOT NULL CONSTRAINT DF_BranchLocation_Disabled DEFAULT(0) WITH VALUES;
Go
UPDATE dbo.Kiosk SET Disabled = 0 WHERE Disabled IS NULL;
ALTER TABLE dbo.Kiosk ALTER COLUMN Disabled bit NOT NULL;


UPDATE dbo.BranchLocation SET Disabled = 0 WHERE Disabled IS NULL;
ALTER TABLE dbo.BranchLocation ALTER COLUMN Disabled bit NOT NULL;
GO
ALTER TABLE dbo.Kiosk ADD CONSTRAINT DF_Kiosk_Disabled DEFAULT(0)  FOR [Disabled];
ALTER TABLE dbo.BranchLocation ADD CONSTRAINT DF_BranchLocation_Disabled DEFAULT(0)  FOR [Disabled];
GO
------------------------------------
alter table dbo.ScreeningResult
	DROP CONSTRAINT FK_ScreeningResult__Kiosk ;
alter table dbo.ScreeningResult	ADD
	CONSTRAINT FK_ScreeningResult__Kiosk FOREIGN KEY(KioskID) 
		REFERENCES dbo.Kiosk(KioskID) 
		ON DELETE NO ACTIOn ON UPDATE CASCADE
---------------------------------------		

alter table ScreeningResult
alter column Phone char(14) not null