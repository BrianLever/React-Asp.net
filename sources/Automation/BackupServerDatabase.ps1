Param(
	[string]$dbname = 'FrontDesk_dev'
)

$date = Get-Date -Format yyyyMMddHHmmss

Backup-SqlDatabase -ServerInstance .\mssqlserver2014  -Database $dbname -BackupFile "D:\SQLDATA\2014\SQLBAKS\$($dbname)_db_$($date).bak"