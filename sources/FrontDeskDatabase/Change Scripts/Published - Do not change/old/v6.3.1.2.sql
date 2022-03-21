IF OBJECT_ID('UQ_SecurityLog') IS NOT NULL
SET NOEXEC ON
GO



ALTER TABLE [dbo].[SecurityLog]
    ADD [ID] [bigint] NOT NULL IDENTITY (1,1);

ALTER TABLE [dbo].[SecurityLog]
    DROP CONSTRAINT [PK_SecurityLog];

ALTER TABLE [dbo].[SecurityLog]
    ADD CONSTRAINT [PK_SecurityLog] PRIMARY KEY ([ID]);

ALTER TABLE [dbo].[SecurityLog]
    ADD CONSTRAINT [UQ_SecurityLog] UNIQUE ([LogDate] ASC, [PKID] ASC, [ID])



SET NOEXEC OFF
GO
;



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.3.1.2')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.3.1.2');