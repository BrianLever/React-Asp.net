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


CREATE NONCLUSTERED INDEX IX__License_ActivateDate
ON [dbo].[License] (ActivatedDate Desc, CreatedDate DESC)

GO