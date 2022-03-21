MERGE into dbo.SystemSettings target
USING ( 
	VALUES 
    ('EHRSystem', 'NEXTGEN', 'External EHR System', 'Integration with external EHR system. Values: RPMS, NEXTGEN, NONE', NULL, 0),
	('CareBridgeAPICredentials', '{"Username":"screendox", "Password":"*******"}', 'CareBridge credentials', '', NULL, 0)
	
    ) as source([Key], Value, Name, Description, RegExp, IsExposed)
ON target.[Key] = source.[Key]
WHEN MATCHED THEN
	UPDATE SET target.value = source.value, target.Name = source.Name, target.Description = source.Description, target.Regexp = source.RegExp, target.IsExposed = source.IsExposed
WHEN NOT MATCHED BY target THEN
	INSERT([Key], Value, Name, Description, RegExp, IsExposed) 
		VALUES(source.[Key], source.Value, source.Name, source.Description, source.RegExp, source.IsExposed)
;



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.3.1.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.3.1.1');