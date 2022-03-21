UPDATE dbo.VisitSettings
SET Name = 'Alcohol Use (CAGE)' WHERE MeasureToolId = 'CAGE';


---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.0.7')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.0.7');