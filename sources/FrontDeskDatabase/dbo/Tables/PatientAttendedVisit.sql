CREATE TABLE [dbo].[PatientAttendedVisit]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_PatientAttendedVisit PRIMARY KEY CLUSTERED (ID),
);
GO

