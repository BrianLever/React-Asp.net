CREATE TABLE [dbo].[SecurityEventCategory](
	[SecurityEventCategoryID] [int] NOT NULL,
	[CategoryName] [nvarchar](128) NOT NULL,
	CONSTRAINT [PK_SecurityEventCategory] PRIMARY KEY CLUSTERED([SecurityEventCategoryID] ASC) 
);
GO
