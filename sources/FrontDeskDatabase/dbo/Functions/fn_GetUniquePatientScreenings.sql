CREATE FUNCTION [dbo].[fn_GetUniquePatientScreenings]
(
    @StartDate DateTimeOffset,
    @EndDate DateTimeOffset,
    @LocationID int  = NULL
)
RETURNS @returntable TABLE
(
    ScreeningResultID bigint,
    CreatedDate DateTimeOffset,
    PatientName nvarchar(max),
    LastName nvarchar(128),
    FirstName nvarchar(128),
    MiddleName nvarchar(128),
    Birthday date
)
AS
BEGIN
    INSERT @returntable
    SELECT
        MAX(r.ScreeningResultID) as ScreeningResultID,
        MAX(r.CreatedDate) as CreatedDate,
        r.PatientName,
        r.LastName,
        r.FirstName,
        ISNULL(r.MiddleName,'') as MiddleName,
        r.Birthday
    FROM dbo.ScreeningResult r
        INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
    WHERE r.IsDeleted = 0
        AND (r.CreatedDate >= @StartDate AND r.CreatedDate <= @EndDate)
        AND (@LocationID IS NULL OR k.BranchLocationID = @LocationID)
    GROUP BY r.Birthday, r.PatientName, r.LastName, r.FirstName, r.MiddleName
    RETURN
END
