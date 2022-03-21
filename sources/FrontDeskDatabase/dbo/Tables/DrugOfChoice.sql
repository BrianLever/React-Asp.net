CREATE TABLE [dbo].[DrugOfChoice]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_DrugOfChoice PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_DrugOfChoice_OrderIndex 
    ON dbo.[DrugOfChoice]([OrderIndex] DESC) INCLUDE([Name])
GO