CREATE TABLE dbo.Users_BranchLocation(
	UserID int NOT NULL,
	BranchLocationID int NOT NULL
		
	CONSTRAINT FK_UsersBranchLocation_UserID FOREIGN KEY(UserID) 
		REFERENCES dbo.Users(PKID) 
			ON UPDATE CASCADE 
			ON DELETE CASCADE,
	CONSTRAINT FK_UsersBranchLocation_BranchLocationID FOREIGN KEY(BranchLocationID) 
		REFERENCES dbo.BranchLocation(BranchLocationID) 
			ON UPDATE NO ACTION 
			ON DELETE NO ACTION,
	
	CONSTRAINT UQ_UsersBranchLocation_UserID UNIQUE(UserID)	
);
