CREATE TABLE [export].[PatientNameCorrectionLog]
(
    [ID] INT NOT NULL IDENTITY(1,1),

    [OriginalPatientName] nvarchar(400) NOT NULL,
    [OriginalBirthday] date NOT NULL,
    [CreatedDate] datetimeoffset NOT NULL,
    [CorrectedPatientName] nvarchar(400) NOT NULL,
    [CorrectedBirthday] date NOT NULL,
    [Comments] nvarchar(max) NOT NULL,

    CONSTRAINT PK__PatientNameCorrectionLog PRIMARY KEY CLUSTERED (ID),
)
GO

CREATE INDEX IX__PatientNameCorrectionLog__CorrectedPatientName 
    ON [export].[PatientNameCorrectionLog]([CorrectedPatientName], [CorrectedBirthday])
GO

CREATE INDEX IX__PatientNameCorrectionLog__CreatedDate 
    ON [export].[PatientNameCorrectionLog](CreatedDate DESC)
GO