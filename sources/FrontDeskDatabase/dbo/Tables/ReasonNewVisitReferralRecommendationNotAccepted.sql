CREATE TABLE [dbo].[ReasonNewVisitReferralRecommendationNotAccepted]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_ReasonNewVisitReferralRecommendationNotAccepted PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_ReasonNewVisitReferralRecommendationNotAccepted_OrderIndex 
    ON dbo.ReasonNewVisitReferralRecommendationNotAccepted([OrderIndex] DESC) INCLUDE([Name])
GO
