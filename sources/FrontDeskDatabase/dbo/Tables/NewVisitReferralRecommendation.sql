CREATE TABLE [dbo].[NewVisitReferralRecommendation]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_NewVisitReferralRecommendation PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_NewVisitReferralRecommendation_OrderIndex 
    ON dbo.[NewVisitReferralRecommendation]([OrderIndex] DESC) INCLUDE([Name])
GO
