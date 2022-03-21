--DECLARE 
    --@StartDate DateTimeOffset = '2019-10-01',
 --   @EndDate DateTimeOffset = '2020-09-30',
 --   @LocationID int  = NULL

    --;

CREATE PROCEDURE [dbo].[uspGetUniqueDrugsOfChoiceScreeningsResultsForExcelExport]
    @StartDate DateTimeOffset,
    @EndDate DateTimeOffset,
    @LocationID int  = NULL
AS

SELECT
r.ScreeningResultID as 'ScreeDox Record No.',
CONVERT(char(10),r.CreatedDate, 101) as 'ScreeningDate',
r.LastName,
r.FirstName,
ISNULL(r.MiddleName,'') as MiddleName,
CONVERT(char(10), r.Birthday, 101) as 'Birthday'
,l.BranchLocationID as 'LocationID'
,l.Name as 'Location'

-- demographics
,ISNULL(demographics.ID,'') as 'DemographicsId'

-- doch
,doch.[Primary Drug]
,doch.[Secondary Drug]
,doch.[Tertiary Drug]
,doch.[Drug Use - ScreeningDate]



FROM 
    [dbo].[fn_GetUniquePatientScreenings] ( @StartDate, @EndDate, @LocationID) as ur
        --doch
    CROSS APPLY (
        SELECT TOP (1)
            r01.ScreeningResultID
            ,doch1a.Name as 'Primary Drug'
            ,doch2a.Name as 'Secondary Drug'
            ,doch3a.Name as 'Tertiary Drug'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'Drug Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionQuestionResult doch1 ON r01.ScreeningResultID = doch1.ScreeningResultID AND doch1.ScreeningSectionID = 'DOCH' AND doch1.QuestionID = 1
                LEFT JOIN dbo.DrugOfChoice doch1a ON doch1.AnswerValue = doch1a.ID
            LEFT JOIN dbo.ScreeningSectionQuestionResult doch2 ON r01.ScreeningResultID = doch2.ScreeningResultID AND doch2.ScreeningSectionID = 'DOCH' AND doch2.QuestionID = 2
                LEFT JOIN dbo.DrugOfChoice doch2a ON ISNULL(doch2.AnswerValue,0) = doch2a.ID
            LEFT JOIN dbo.ScreeningSectionQuestionResult doch3 ON r01.ScreeningResultID = doch3.ScreeningResultID AND doch3.ScreeningSectionID = 'DOCH' AND doch3.QuestionID = 3
                LEFT JOIN dbo.DrugOfChoice doch3a ON ISNULL(doch3.AnswerValue,0) = doch3a.ID
        WHERE r01.PatientName = ur.PatientName AND r01.Birthday = ur.Birthday
                        AND doch1a.Name is NOT NULL
                        AND r01.CreatedDate <= ur.CreatedDate
        ORDER BY r01.CreatedDate DESC
    ) as doch
    INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = doch.ScreeningResultID
    INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = k.BranchLocationID

    -- demographics
    LEFT JOIN (SELECT MAX(d.ID) as ID, d.PatientName, d.Birthday FROM dbo.BhsDemographics d GROUP BY d.PatientName, d.Birthday) demographics ON  demographics.PatientName = r.PatientName AND demographics.Birthday = r.Birthday
ORDER BY r.CreatedDate ASC

RETURN 0
