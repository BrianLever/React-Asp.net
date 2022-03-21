CREATE TABLE [dbo].[TreatmentAction]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_TreatmentAction PRIMARY KEY CLUSTERED (ID),
);
GO
