CREATE VIEW [dbo].[vDrugsOfChoiceForExcelExport]
AS 
SELECT 
[ScreeDox Record No.],
[ScreeningDate],
[LastName],
[FirstName],
[MiddleName],
[Birthday],
[LocationID],
[Location],
[DemographicsId],
[Primary Drug],
[Secondary Drug],
[Tertiary Drug]

FROM dbo.vScreeningResultsForExcelExport r
WHERE [Primary Drug] is NOT NULL

GO