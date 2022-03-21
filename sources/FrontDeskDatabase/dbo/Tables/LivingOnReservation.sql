CREATE TABLE [dbo].[LivingOnReservation]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__LivingOnReservation__LastModifiedDateUTC DEFAULT GETUTCDATE(),
    CONSTRAINT PK_LivingOnReservation PRIMARY KEY CLUSTERED (ID),
);
GO
