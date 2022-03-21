CREATE TABLE [export].[PatientNameMap]
(
    ID INT NOT NULL IDENTITY(1,1),
    Source nvarchar(128) NOT NULL,
    Destination nvarchar(128) NOT NULL,
    
    CONSTRAINT PK__PatientNameMap PRIMARY KEY CLUSTERED (ID)
);

GO

CREATE INDEX IX__PatientNameMap__Source ON [export].[PatientNameMap](Source);
GO
-- INSERT INTO export.PatientNameMap(Source, Destination) VALUES('TDST', 'TEST');
