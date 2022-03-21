--DECLARE 
    --@StartDate DateTimeOffset = '2019-10-01',
 --   @EndDate DateTimeOffset = '2020-09-30',
 --   @LocationID int  = NULL

    --;

CREATE PROCEDURE [dbo].[uspGetUniquePatientCombinedResultsForExcelExport]
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


-- tobacco
,tcc.[Tobacco Use - Ceremony]
,tcc.[Tobacco Use - Smoking]
,tcc.[Tobacco Use - Smokeless]
,tcc.[Tobaco Use - ScreeningDate]

,cage.[CAGE / Alcohol Use - Q1]
,cage.[CAGE / Alcohol Use - Q2]
,cage.[CAGE / Alcohol Use - Q3]
,cage.[CAGE / Alcohol Use - Q4]
,cage.[CAGE / Alcohol Use - Total]
,cage.[CAGE / Alcohol Use - Score Level]
,[CAGE / Alcohol Use - ScreeningDate]


-- dast
,dast.[DAST-10 / Drug Use - Q1]
,dast.[DAST-10 / Drug Use - Q2]
,dast.[DAST-10 / Drug Use - Q3]
,dast.[DAST-10 / Drug Use - Q4]
,dast.[DAST-10 / Drug Use - Q5]
,dast.[DAST-10 / Drug Use - Q6]
,dast.[DAST-10 / Drug Use - Q7]
,dast.[DAST-10 / Drug Use - Q8]
,dast.[DAST-10 / Drug Use - Q9]
,dast.[DAST-10 / Drug Use - Q10]
,dast.[DAST-10 / Drug Use - Total]
,dast.[DAST-10 / Drug Use - Score Level]
,dast.[DAST-10 / Drug Use - ScreeningDate]

--phq
,phq.[PHQ-9 / Depression - Q1]
,phq.[PHQ-9 / Depression - Q2]
,phq.[PHQ-9 / Depression - Q3]
,phq.[PHQ-9 / Depression - Q4]
,phq.[PHQ-9 / Depression - Q5]
,phq.[PHQ-9 / Depression - Q6]
,phq.[PHQ-9 / Depression - Q7]
,phq.[PHQ-9 / Depression - Q8]
,phq.[PHQ-9 / Depression - Q9]
,phq.[PHQ-9 / Depression - Q10]

,phq.[PHQ-9 / Depression - Total]
,phq.[PHQ-9 / Depression - Score Level]
,phq.[PHQ-9 / Depression - ScreeningDate]


--hits
,hits.[HITS / Violence - Q1]
,hits.[HITS / Violence - Q2]
,hits.[HITS / Violence - Q3]
,hits.[HITS / Violence - Q4]

,hits.[HITS / Violence - Total]
,hits.[HITS / Violence - Score Level]
,hits.[HITS / Violence - ScreeningDate]

-- doch
,doch.[Primary Drug]
,doch.[Secondary Drug]
,doch.[Tertiary Drug]
,doch.[Drug Use - ScreeningDate]

--gad
,gad.[GAD-7 / Anxiety - Q1]
,gad.[GAD-7 / Anxiety - Q2]
,gad.[GAD-7 / Anxiety - Q3]
,gad.[GAD-7 / Anxiety - Q4]
,gad.[GAD-7 / Anxiety - Q5]
,gad.[GAD-7 / Anxiety - Q6]
,gad.[GAD-7 / Anxiety - Q7]
,gad.[GAD-7 / Anxiety - Q8]

,gad.[GAD-7 / Anxiety - Total]
,gad.[GAD-7 / Anxiety - Score Level]
,gad.[GAD-7 / Anxiety - ScreeningDate]


--bbgs
,bbgs.[BBGS / Problem Gambling - Q1]
,bbgs.[BBGS / Problem Gambling - Q2]
,bbgs.[BBGS / Problem Gambling - Q3]
,bbgs.[BBGS / Problem Gambling - Q4]

,bbgs.[BBGS / Problem Gambling - Total]
,bbgs.[BBGS / Problem Gambling - Score Level]
,bbgs.[BBGS / Problem Gambling - ScreeningDate]

-- demographics
,ISNULL(demographics.ID,'') as 'DemographicsId'
,RaceId
,RaceName
,GenderId
,GenderName
,SexualOrientationId
,SexualOrientationName
,TribalAffiliation
,MaritalStatusId
,MaritalStatusName
,EducationLevelId
,EducationLevelName
,LivingOnReservationId
,LivingOnReservationName
,CountyOfResidence
,MilitaryExperienceId
,MilitaryExperienceName

FROM 
    [dbo].[fn_GetUniquePatientScreenings] ( @StartDate, @EndDate, @LocationID) as ur
    INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = ur.ScreeningResultID
    INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = k.BranchLocationID
    
    -- tobacco
    OUTER APPLY (
        SELECT TOP (1)
            q1.AnswerValue as 'Tobacco Use - Ceremony'
            ,q2.AnswerValue as 'Tobacco Use - Smoking'
            ,q3.AnswerValue as 'Tobacco Use - Smokeless'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'Tobaco Use - ScreeningDate'
        
        FROM 
            dbo.ScreeningResult r01 
            INNER JOIN dbo.ScreeningSectionQuestionResult q4 ON r01.ScreeningResultID = q4.ScreeningResultID AND q4.ScreeningSectionID = 'TCC' AND q4.QuestionID = 4
            LEFT JOIN dbo.ScreeningSectionQuestionResult q1 ON r01.ScreeningResultID = q1.ScreeningResultID AND q1.ScreeningSectionID = 'TCC' AND q1.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult q2 ON r01.ScreeningResultID = q2.ScreeningResultID AND q2.ScreeningSectionID = 'TCC' AND q2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult q3 ON r01.ScreeningResultID = q3.ScreeningResultID AND q3.ScreeningSectionID = 'TCC' AND q3.QuestionID = 3 
        WHERE r01.PatientName = ur.PatientName AND r01.Birthday = ur.Birthday 
            AND r01.CreatedDate <= r.CreatedDate
        ORDER BY r01.CreatedDate DESC
    ) as tcc

    --cage
    OUTER APPLY 
    (
        SELECT TOP (1)
            c1.AnswerValue as 'CAGE / Alcohol Use - Q1'
            ,c2.AnswerValue as 'CAGE / Alcohol Use - Q2'
            ,c3.AnswerValue as 'CAGE / Alcohol Use - Q3'
            ,c4.AnswerValue as 'CAGE / Alcohol Use - Q4'
            ,c0.Score as 'CAGE / Alcohol Use - Total'
            ,c0.ScoreLevel as 'CAGE / Alcohol Use - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'CAGE / Alcohol Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult c0 ON r01.ScreeningResultID = c0.ScreeningResultID AND c0.ScreeningSectionID = 'CAGE'

            LEFT JOIN dbo.ScreeningSectionQuestionResult c1 ON r01.ScreeningResultID = c1.ScreeningResultID AND c1.ScreeningSectionID = 'CAGE' AND c1.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c2 ON r01.ScreeningResultID = c2.ScreeningResultID AND c2.ScreeningSectionID = 'CAGE' AND c2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c3 ON r01.ScreeningResultID = c3.ScreeningResultID AND c3.ScreeningSectionID = 'CAGE' AND c3.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult c4 ON r01.ScreeningResultID = c4.ScreeningResultID AND c4.ScreeningSectionID = 'CAGE' AND c4.QuestionID = 4 
    
            
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
            AND r01.CreatedDate <= r.CreatedDate
        ORDER BY r01.CreatedDate DESC
    ) as cage

    -- dast
    OUTER APPLY 
    (
        SELECT TOP (1)
            d1.AnswerValue as 'DAST-10 / Drug Use - Q1'
            ,d2.AnswerValue as 'DAST-10 / Drug Use - Q2'
            ,ABS(d3.AnswerValue - 1) as 'DAST-10 / Drug Use - Q3'
            ,d4.AnswerValue as 'DAST-10 / Drug Use - Q4'
            ,d5.AnswerValue as 'DAST-10 / Drug Use - Q5'
            ,d6.AnswerValue as 'DAST-10 / Drug Use - Q6'
            ,d7.AnswerValue as 'DAST-10 / Drug Use - Q7'
            ,d8.AnswerValue as 'DAST-10 / Drug Use - Q8'
            ,d9.AnswerValue as 'DAST-10 / Drug Use - Q9'
            ,d10.AnswerValue as 'DAST-10 / Drug Use - Q10'

            ,d0.Score as 'DAST-10 / Drug Use - Total'
            ,d0.ScoreLevel as 'DAST-10 / Drug Use - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'DAST-10 / Drug Use - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult d0 ON r01.ScreeningResultID = d0.ScreeningResultID AND d0.ScreeningSectionID = 'DAST'
            INNER JOIN dbo.ScreeningSectionQuestionResult d1 ON r01.ScreeningResultID = d1.ScreeningResultID AND d1.ScreeningSectionID = 'DAST' AND d1.QuestionID = 10 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d2 ON r01.ScreeningResultID = d2.ScreeningResultID AND d2.ScreeningSectionID = 'DAST' AND d2.QuestionID = 1 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d3 ON r01.ScreeningResultID = d3.ScreeningResultID AND d3.ScreeningSectionID = 'DAST' AND d3.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d4 ON r01.ScreeningResultID = d4.ScreeningResultID AND d4.ScreeningSectionID = 'DAST' AND d4.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d5 ON r01.ScreeningResultID = d5.ScreeningResultID AND d5.ScreeningSectionID = 'DAST' AND d5.QuestionID = 4 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d6 ON r01.ScreeningResultID = d6.ScreeningResultID AND d6.ScreeningSectionID = 'DAST' AND d6.QuestionID = 5 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d7 ON r01.ScreeningResultID = d7.ScreeningResultID AND d7.ScreeningSectionID = 'DAST' AND d7.QuestionID = 6 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d8 ON r01.ScreeningResultID = d8.ScreeningResultID AND d8.ScreeningSectionID = 'DAST' AND d8.QuestionID = 7 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d9 ON r01.ScreeningResultID = d9.ScreeningResultID AND d9.ScreeningSectionID = 'DAST' AND d9.QuestionID = 8 	 
            LEFT JOIN dbo.ScreeningSectionQuestionResult d10 ON r01.ScreeningResultID = d10.ScreeningResultID AND d10.ScreeningSectionID = 'DAST' AND d10.QuestionID = 9 	 
            
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                    AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as dast


    --phq
    OUTER APPLY 
    (
    SELECT TOP (1)
        p1.AnswerValue as 'PHQ-9 / Depression - Q1'
        ,p2.AnswerValue as 'PHQ-9 / Depression - Q2'
        ,p3.AnswerValue as 'PHQ-9 / Depression - Q3'
        ,p4.AnswerValue as 'PHQ-9 / Depression - Q4'
        ,p5.AnswerValue as 'PHQ-9 / Depression - Q5'
        ,p6.AnswerValue as 'PHQ-9 / Depression - Q6'
        ,p7.AnswerValue as 'PHQ-9 / Depression - Q7'
        ,p8.AnswerValue as 'PHQ-9 / Depression - Q8'
        ,p9.AnswerValue as 'PHQ-9 / Depression - Q9'
        ,p10a.OptionText as 'PHQ-9 / Depression - Q10'
        ,p0.Score as 'PHQ-9 / Depression - Total'
        ,p0.ScoreLevel as 'PHQ-9 / Depression - Score Level'
        ,CONVERT(char(10), r01.CreatedDate, 101) as 'PHQ-9 / Depression - ScreeningDate'

    FROM dbo.ScreeningResult r01
        INNER JOIN dbo.ScreeningSectionResult p0 ON r01.ScreeningResultID = p0.ScreeningResultID AND p0.ScreeningSectionID = 'PHQ-9'
        INNER JOIN dbo.ScreeningSectionQuestionResult p1 ON r01.ScreeningResultID = p1.ScreeningResultID AND p1.ScreeningSectionID = 'PHQ-9' AND p1.QuestionID = 1 
        INNER JOIN dbo.ScreeningSectionQuestionResult p2 ON r01.ScreeningResultID = p2.ScreeningResultID AND p2.ScreeningSectionID = 'PHQ-9' AND p2.QuestionID = 2 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p3 ON r01.ScreeningResultID = p3.ScreeningResultID AND p3.ScreeningSectionID = 'PHQ-9' AND p3.QuestionID = 3 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p4 ON r01.ScreeningResultID = p4.ScreeningResultID AND p4.ScreeningSectionID = 'PHQ-9' AND p4.QuestionID = 4 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p5 ON r01.ScreeningResultID = p5.ScreeningResultID AND p5.ScreeningSectionID = 'PHQ-9' AND p5.QuestionID = 5 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p6 ON r01.ScreeningResultID = p6.ScreeningResultID AND p6.ScreeningSectionID = 'PHQ-9' AND p6.QuestionID = 6 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p7 ON r01.ScreeningResultID = p7.ScreeningResultID AND p7.ScreeningSectionID = 'PHQ-9' AND p7.QuestionID = 7 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p8 ON r01.ScreeningResultID = p8.ScreeningResultID AND p8.ScreeningSectionID = 'PHQ-9' AND p8.QuestionID = 8 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p9 ON r01.ScreeningResultID = p9.ScreeningResultID AND p9.ScreeningSectionID = 'PHQ-9' AND p9.QuestionID = 9 
        LEFT JOIN dbo.ScreeningSectionQuestionResult p10 ON r01.ScreeningResultID = p10.ScreeningResultID AND p10.ScreeningSectionID = 'PHQ-9' AND p10.QuestionID = 10
            LEFT JOIN dbo.ScreeningSectionQuestion p10q ON p10q.ScreeningSectionID = p10.ScreeningSectionID AND p10q.QuestionID = p10.QuestionID
            LEFT JOIN dbo.AnswerScaleOption p10a ON p10a.AnswerScaleID = p10q.AnswerScaleID AND p10a.OptionValue = p10.AnswerValue
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC

    ) as phq

            
    --hits
    OUTER APPLY (
        SELECT TOP(1)
        h1.AnswerValue as 'HITS / Violence - Q1'
        ,h2.AnswerValue as 'HITS / Violence - Q2'
        ,h3.AnswerValue as 'HITS / Violence - Q3'
        ,h4.AnswerValue as 'HITS / Violence - Q4'

        ,h0.Score as 'HITS / Violence - Total'
        ,h0.ScoreLevel as 'HITS / Violence - Score Level'
        ,CONVERT(char(10), r01.CreatedDate, 101) as 'HITS / Violence - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult h0 ON r01.ScreeningResultID = h0.ScreeningResultID AND h0.ScreeningSectionID = 'HITS'
            LEFT JOIN dbo.ScreeningSectionQuestionResult h1 ON r01.ScreeningResultID = h1.ScreeningResultID AND h1.ScreeningSectionID = 'HITS' AND h1.QuestionID = 1
            LEFT JOIN dbo.ScreeningSectionQuestionResult h2 ON r01.ScreeningResultID = h2.ScreeningResultID AND h2.ScreeningSectionID = 'HITS' AND h2.QuestionID = 2
            LEFT JOIN dbo.ScreeningSectionQuestionResult h3 ON r01.ScreeningResultID = h3.ScreeningResultID AND h3.ScreeningSectionID = 'HITS' AND h3.QuestionID = 3
            LEFT JOIN dbo.ScreeningSectionQuestionResult h4 ON r01.ScreeningResultID = h4.ScreeningResultID AND h4.ScreeningSectionID = 'HITS' AND h4.QuestionID = 4
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as hits

        --doch
    OUTER APPLY (
        SELECT TOP (1)
            doch1a.Name as 'Primary Drug'
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
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as doch


    -- gad
    OUTER APPLY 
    (
        SELECT TOP (1)
            p1.AnswerValue as 'GAD-7 / Anxiety - Q1'
            ,p2.AnswerValue as 'GAD-7 / Anxiety - Q2'
            ,p3.AnswerValue as 'GAD-7 / Anxiety - Q3'
            ,p4.AnswerValue as 'GAD-7 / Anxiety - Q4'
            ,p5.AnswerValue as 'GAD-7 / Anxiety - Q5'
            ,p6.AnswerValue as 'GAD-7 / Anxiety - Q6'
            ,p7.AnswerValue as 'GAD-7 / Anxiety - Q7'
            ,p8a.OptionText as 'GAD-7 / Anxiety - Q8'
            ,p0.Score as 'GAD-7 / Anxiety - Total'
            ,p0.ScoreLevel as 'GAD-7 / Anxiety - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'GAD-7 / Anxiety - ScreeningDate'

        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult p0 ON r01.ScreeningResultID = p0.ScreeningResultID AND p0.ScreeningSectionID = 'GAD-7'
            INNER JOIN dbo.ScreeningSectionQuestionResult p1 ON r01.ScreeningResultID = p1.ScreeningResultID AND p1.ScreeningSectionID = 'GAD-7' AND p1.QuestionID = 1 
            INNER JOIN dbo.ScreeningSectionQuestionResult p2 ON r01.ScreeningResultID = p2.ScreeningResultID AND p2.ScreeningSectionID = 'GAD-7' AND p2.QuestionID = 2 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p3 ON r01.ScreeningResultID = p3.ScreeningResultID AND p3.ScreeningSectionID = 'GAD-7' AND p3.QuestionID = 3 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p4 ON r01.ScreeningResultID = p4.ScreeningResultID AND p4.ScreeningSectionID = 'GAD-7' AND p4.QuestionID = 4 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p5 ON r01.ScreeningResultID = p5.ScreeningResultID AND p5.ScreeningSectionID = 'GAD-7' AND p5.QuestionID = 5 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p6 ON r01.ScreeningResultID = p6.ScreeningResultID AND p6.ScreeningSectionID = 'GAD-7' AND p6.QuestionID = 6 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p7 ON r01.ScreeningResultID = p7.ScreeningResultID AND p7.ScreeningSectionID = 'GAD-7' AND p7.QuestionID = 7 
            LEFT JOIN dbo.ScreeningSectionQuestionResult p8 ON r01.ScreeningResultID = p8.ScreeningResultID AND p8.ScreeningSectionID = 'GAD-7' AND p8.QuestionID = 8
                LEFT JOIN dbo.ScreeningSectionQuestion p8q ON p8q.ScreeningSectionID = p8.ScreeningSectionID AND p8q.QuestionID = p8.QuestionID
                LEFT JOIN dbo.AnswerScaleOption p8a ON p8a.AnswerScaleID = p8q.AnswerScaleID AND p8a.OptionValue = p8.AnswerValue
    
            WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                            AND r01.CreatedDate <= r.CreatedDate 
            ORDER BY r01.CreatedDate DESC
    ) as gad

     --bbgs
    OUTER APPLY (
        SELECT TOP(1)
            h1.AnswerValue as 'BBGS / Problem Gambling - Q1'
            ,h2.AnswerValue as 'BBGS / Problem Gambling - Q2'
            ,h3.AnswerValue as 'BBGS / Problem Gambling - Q3'
            ,h4.AnswerValue as 'BBGS / Problem Gambling - Q4'

            ,h0.Score as 'BBGS / Problem Gambling - Total'
            ,h0.ScoreLevel as 'BBGS / Problem Gambling - Score Level'
            ,CONVERT(char(10), r01.CreatedDate, 101) as 'BBGS / Problem Gambling - ScreeningDate'
        FROM dbo.ScreeningResult r01
            INNER JOIN dbo.ScreeningSectionResult h0 ON r01.ScreeningResultID = h0.ScreeningResultID AND h0.ScreeningSectionID = 'BBGS'
            LEFT JOIN dbo.ScreeningSectionQuestionResult h1 ON r01.ScreeningResultID = h1.ScreeningResultID AND h1.ScreeningSectionID = 'BBGS' AND h1.QuestionID = 1
            LEFT JOIN dbo.ScreeningSectionQuestionResult h2 ON r01.ScreeningResultID = h2.ScreeningResultID AND h2.ScreeningSectionID = 'BBGS' AND h2.QuestionID = 2
            LEFT JOIN dbo.ScreeningSectionQuestionResult h3 ON r01.ScreeningResultID = h3.ScreeningResultID AND h3.ScreeningSectionID = 'BBGS' AND h3.QuestionID = 3
            LEFT JOIN dbo.ScreeningSectionQuestionResult h4 ON r01.ScreeningResultID = h4.ScreeningResultID AND h4.ScreeningSectionID = 'BBGS' AND h4.QuestionID = 4
    
        WHERE r01.PatientName = r.PatientName AND r01.Birthday = r.Birthday
                        AND r01.CreatedDate <= r.CreatedDate 
        ORDER BY r01.CreatedDate DESC
    ) as bbgs

    -- demographics
    OUTER APPLY (
        SELECT TOP (1) 
            ID
            ,RaceId
            ,RaceName
            ,GenderId
            ,GenderName
            ,SexualOrientationId
            ,SexualOrientationName
            ,TribalAffiliation
            ,MaritalStatusId
            ,MaritalStatusName
            ,EducationLevelId
            ,EducationLevelName
            ,LivingOnReservationId
            ,LivingOnReservationName
            ,CountyOfResidence
            ,MilitaryExperienceId
            ,MilitaryExperienceName
        FROM [dbo].[vBhsDemographics] d
        WHERE d.FullName = r.PatientName AND d.Birthday = r.Birthday
    ) as demographics
ORDER BY ur.CreatedDate ASC

RETURN 0
