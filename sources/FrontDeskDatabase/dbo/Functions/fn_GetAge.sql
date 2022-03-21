CREATE FUNCTION [dbo].[fn_GetAge]
(
	@Birthday date
)
RETURNS int AS 
BEGIN

DECLARE @CurrentDate DATETIME = GETDATE();


 RETURN CASE WHEN dateadd(year, datediff (year, @Birthday, @CurrentDate), @Birthday) > @CurrentDate
        THEN datediff(year, @Birthday, @CurrentDate) - 1
        ELSE datediff(year, @Birthday, @CurrentDate)
    END
END
GO
