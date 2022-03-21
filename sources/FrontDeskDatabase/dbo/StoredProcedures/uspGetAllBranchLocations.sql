CREATE PROCEDURE [dbo].[uspGetAllBranchLocations]
    @HideDisabled bit = 0
AS
    SELECT
        l.BranchLocationID,
        l.Name,
        l.Description,
        l.Disabled,
        p.ID as ScreeningProfileID,
        p.Name as ScreeningProfileName
    FROM dbo.BranchLocation l
        INNER JOIN dbo.ScreeningProfile p ON l.ScreeningProfileID = p.ID
    WHERE @HideDisabled = 0 OR l.Disabled = 0
    ORDER BY l.Name ASC
RETURN 0
GO
