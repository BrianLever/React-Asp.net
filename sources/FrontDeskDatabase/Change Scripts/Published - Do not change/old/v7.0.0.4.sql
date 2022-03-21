--- Gender

IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.Gender') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.Gender ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__Gender__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO

-- Race
IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.Race') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.Race ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__Race__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO


-- MilitaryExperience
IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.MilitaryExperience') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.MilitaryExperience ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__MilitaryExperience__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO

-- County
IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.County') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.County ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__County__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO


-- LivingOnReservation
IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.LivingOnReservation') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.LivingOnReservation ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__LivingOnReservation__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO


-- MaritalStatus
IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.MaritalStatus') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.MaritalStatus ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__MaritalStatus__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO

-- Tribe
IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.Tribe') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.Tribe ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__Tribe__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO

-- SexualOrientation
IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.SexualOrientation') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.SexualOrientation ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__SexualOrientation__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO


-- EducationLevel
IF EXISTS(SELECT 1 from sys.columns 
        WHERE object_id = OBJECT_ID('dbo.EducationLevel') and name = 'LastModifiedDateUTC')
SET NOEXEC ON;
GO

ALTER TABLE dbo.EducationLevel ADD LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__EducationLevel__LastModifiedDateUTC DEFAULT GETUTCDATE()
;

SET NOEXEC OFF
GO

---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.0.4')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.0.4');