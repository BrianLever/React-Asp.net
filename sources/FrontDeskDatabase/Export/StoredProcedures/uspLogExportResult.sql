CREATE PROCEDURE [export].[uspLogExportResult]
    @ScreeningResultID bigint,
    @Succeed int,
    @ExportDate DateTimeOffset,
    @FailedAttemptCount int,
    @FailedReason nvarchar(128),
    @LastError nvarchar(max),
    @Completed bit
AS
MERGE INTO [export].[SmartExportLog] as target
USING (
    SELECT @ScreeningResultID, @Succeed, @ExportDate, @FailedAttemptCount, @FailedReason, @LastError, @Completed) 
	as source(ScreeningResultID, Succeed, ExportDate, FailedAttemptCount, FailedReason, LastError, Completed)
    ON (target.ScreeningResultID = source.ScreeningResultID)  

WHEN MATCHED THEN UPDATE 
	SET target.Succeed = source.Succeed,
	target.ExportDate = source.ExportDate,
    target.FailedAttemptCount = ISNULL(source.FailedAttemptCount, target.FailedAttemptCount),
    target.FailedReason = ISNULL(source.FailedReason, target.FailedReason),
    target.LastError = ISNULL(source.LastError, target.LastError),
	target.Completed = source.Completed
WHEN NOT MATCHED BY TARGET THEN   
	INSERT ([ScreeningResultID]
           ,[Succeed]
           ,[ExportDate]
           ,[FailedAttemptCount]
           ,[FailedReason]
           ,[LastError]
           ,[Completed])
     VALUES
           (source.ScreeningResultID
           ,source.Succeed 
           ,source.ExportDate
           ,source.FailedAttemptCount
           ,source.FailedReason
           ,source.LastError
           ,source.Completed
           )
;

RETURN 0

go