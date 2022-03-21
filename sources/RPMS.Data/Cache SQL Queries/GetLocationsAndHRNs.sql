--GET ALL LOCATIONS
select l.RowId as LocationID,
i.Name, 
l.SHORT_NAME
from  IHS.LOCATION l 
INNER JOIN IHS.INSTITUTION i ON l.Name = i.RowId




