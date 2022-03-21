CREATE TABLE [dbo].[SecurityLog](
	[PKID] [int] NOT NULL,
	[LogDate] [datetimeoffset](7) NOT NULL,
	[SecurityEventID] [int] NOT NULL,
	[Metadata] [sql_variant] NULL,
	[RelatedBranchID] [int] NULL,
	[ID] [bigint] NOT NULL IDENTITY (1,1),
    CONSTRAINT [PK_SecurityLog] PRIMARY KEY ([ID]),
    CONSTRAINT [UQ_SecurityLog] UNIQUE NONCLUSTERED([LogDate] ASC, [PKID] ASC, [ID]),
	CONSTRAINT [FK_SecurityLog_BranchLocationID] FOREIGN KEY([RelatedBranchID])
		REFERENCES [dbo].[BranchLocation] ([BranchLocationID])
		ON UPDATE CASCADE
		ON DELETE SET NULL,
	CONSTRAINT [FK_SecurityLog_SecurityLogAction] FOREIGN KEY([SecurityEventID]) 
		REFERENCES [dbo].[SecurityEvent] ([SecurityEventID]),
	CONSTRAINT [FK_SecurityLog_Users] FOREIGN KEY([PKID])
		REFERENCES [dbo].[Users] ([PKID])
	
)
GO
