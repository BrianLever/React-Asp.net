CREATE PROCEDURE [dbo].[uspGetBranchLocation]
    @BranchLocationID int
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
    WHERE BranchLocationID = @BranchLocationID
RETURN 0
GO
