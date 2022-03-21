CREATE FUNCTION [dbo].[ufnMapPatientName]
(
    @Source nvarchar(128)
)
RETURNS nvarchar(128)
AS
BEGIN
    DECLARE @Dest nvarchar(128)
    
    SELECT @Dest = Destination FROM export.PatientNameMap WHERE Source = @Source;

    
    RETURN ISNULL(@Dest, UPPER(@Source))
END
