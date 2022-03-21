CREATE TABLE [dbo].[Discharged]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_Discharged PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_Discharged_OrderIndex 
    ON dbo.[Discharged]([OrderIndex] DESC) INCLUDE([Name])
GO
