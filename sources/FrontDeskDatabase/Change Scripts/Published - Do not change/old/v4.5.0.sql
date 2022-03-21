-- Insert new section for Minimal Age - Contact Information


IF NOT EXISTS(SELECT 1 FROM ScreeningSection WHERE ScreeningSectionID = 'CIF')
BEGIN
	
	UPDATE ScreeningSection SET OrderIndex = OrderIndex + 1

	INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionShortName, ScreeningSectionName,QuestionText, OrderIndex)
	VALUES
	('CIF', 'BHS', 'CIF', 'Contact Information', '', 0);

END

IF NOT EXISTS(SELECT 1 FROM ScreeningSectionAge WHERE ScreeningSectionID = 'CIF')
	INSERT INTO ScreeningSectionAge(ScreeningSectionID, MinimalAge, IsEnabled, LastModifiedDateUTC) VALUES
	('CIF', 0, 1, GETUTCDATE());


IF NOT EXISTS(SELECT 1 FROM ScreeningFrequency WHERE ID = 'CIF')
BEGIN
	
	INSERT INTO ScreeningFrequency(ID, Frequency, LastModifiedDateUTC) values('CIF', 0, GETUTCDATE());

	DELETE FROM ScreeningFrequency WHERE ID = '_Contact';
END



IF OBJECT_ID('[dbo].[fn_GetAge]') IS NOT NULL
SET NOEXEC ON
GO
CREATE FUNCTION [dbo].[fn_GetAge]() RETURNS INT AS BEGIN RETURN 0 END
GO

SET NOEXEC OFF

GO

ALTER FUNCTION [dbo].[fn_GetAge]
(
	@Birthday date
)
RETURNS int AS 
BEGIN

DECLARE @CurrentDate DATETIME = GETDATE();


 RETURN CASE WHEN dateadd(year, datediff (year, @Birthday, @CurrentDate), @Birthday) > @CurrentDate
        THEN datediff(year, @Birthday, @CurrentDate) - 1
        ELSE datediff(year, @Birthday, @CurrentDate)
    END
END
GO

GRANT EXECUTE  ON [dbo].[fn_GetAge] TO frontdesk_appuser
GO
GRANT EXECUTE  ON [dbo].[fn_GetAge] TO frontdesk_appaccount


----------------------------------------
INSERT INTO dbo.DbVersion(DbVersion) VALUES('4.5.0.0');
