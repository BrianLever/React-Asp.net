CREATE TABLE [dbo].[NewVisitReferralRecommendationAccepted]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_NewVisitReferralRecommendationAccepted PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_NewVisitReferralRecommendationAccepted_OrderIndex 
    ON dbo.NewVisitReferralRecommendationAccepted([OrderIndex] DESC) INCLUDE([Name])
GO
