-- MIGRATE PHQ-9 records to section type PHQ9A in ScreeningTimeLog

begin transaction

select * from dbo.ScreeningTimeLog where ScreeningSectionID IN ('PHQ-9', 'PHQ9A');
GO
WITH cte (ID, ScreeningResultID, ScreeningSectionID)AS (
select tl.ID, tl.ScreeningResultID, tl.ScreeningSectionID
from dbo.ScreeningTimeLog tl
	INNER JOIN dbo.ScreeningSectionResult sr 
		ON tl.ScreeningResultID = sr.ScreeningResultID AND tl.ScreeningSectionID = sr.ScreeningSectionID
	CROSS APPLY (
		SELECT count(*) as cnt from dbo.ScreeningSectionQuestionResult qr
		WHERE qr.ScreeningResultID = sr.ScreeningResultID AND
			qr.ScreeningSectionID = sr.ScreeningSectionID
			) as q 
	where tl.ScreeningSectionID = 'PHQ-9' and q.cnt = 10
)
UPDATE tl
SET ScreeningSectionID = 'PHQ9A'
FROM ScreeningTimeLog tl INNER JOIN cte 
	ON tl.ID = cte.ID
		AND tl.ScreeningResultID = cte.ScreeningResultID

GO


select * from dbo.ScreeningTimeLog where ScreeningSectionID IN ('PHQ-9', 'PHQ9A');

commit transaction


---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.1.0.2')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.1.0.2');