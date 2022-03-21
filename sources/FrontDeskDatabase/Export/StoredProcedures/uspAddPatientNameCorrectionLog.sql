CREATE PROCEDURE [export].[uspAddPatientNameCorrectionLog]
    @OriginalPatientName nvarchar(400),
    @OriginalBirthday date,
    @CreatedDate datetimeoffset,
    @CorrectedPatientName nvarchar(400),
    @CorrectedBirthday date,
    @Comments nvarchar(max)
AS
INSERT INTO [export].[PatientNameCorrectionLog] (
    [OriginalPatientName],
    [OriginalBirthday],
    [CreatedDate],
    [CorrectedPatientName],
    [CorrectedBirthday],
    [Comments]
)
VALUES(
    @OriginalPatientName,
    @OriginalBirthday,
    @CreatedDate,
    @CorrectedPatientName,
    @CorrectedBirthday,
    @Comments
)
GO
