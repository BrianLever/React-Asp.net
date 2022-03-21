begin transaction

INSERT INTO [export].[SmartExportLog]
           ([ScreeningResultID]
           ,[Succeed]
           ,[ExportDate]
           ,[FailedAttemptCount]
           ,[FailedReason]
           ,[LastError]
           ,[Completed])
 SELECT    
        r.ScreeningResultID, 
        0 as Succeed,
        r.CreatedDate as ExportDate,
        0 as FailedAttemptCount,
        'Excluded from auto-export' as FailedReason,
        NULL as LastError,
        1 as Completed
    FROM dbo.ScreeningResult r
        LEFT JOIN export.SmartExportLog el ON r.ScreeningResultID = el.ScreeningResultID 
   WHERE r.IsDeleted = 0 AND r.ExportDate IS NULL AND el.ScreeningResultID IS NULL
   AND CreatedDate < '2019-10-31 0:0:0 -07:00'     
   
   
   
   select * from [export].[SmartExportLog]
   where Completed = 1
   order by [ExportDate] DESC
   
   
   commit transaction
 


