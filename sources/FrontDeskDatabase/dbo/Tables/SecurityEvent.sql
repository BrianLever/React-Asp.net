CREATE TABLE [dbo].[SecurityEvent](
	[SecurityEventID] [int] NOT NULL,
	[SecurityEventCategoryID] [int] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Enabled] [bit] NOT NULL CONSTRAINT DF_SecurityEvent_Enabled DEFAULT(0)
	CONSTRAINT [PK_SecurityLogAction] PRIMARY KEY CLUSTERED ([SecurityEventID] ASC),
	CONSTRAINT [FK_SecurityEvent_SecurityEventCategory] FOREIGN KEY([SecurityEventCategoryID])
		REFERENCES [dbo].[SecurityEventCategory] ([SecurityEventCategoryID])
		ON DELETE NO ACTION ON UPDATE NO ACTION
)
GO
