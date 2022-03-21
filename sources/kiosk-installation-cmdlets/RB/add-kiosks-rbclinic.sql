begin transaction

SET IDEnTITY_INSERT dbo.Kiosk ON

--INSERT INTO [dbo].[Kiosk](KioskID, SecretKey, [KioskName],[Description],[BranchLocationID],[Disabled], CreatedDate) VALUES($1, '$2', 'S/N:$5', 'Model: $3;  OS: $4;  Label: $5; Name: $6',1,0, SYSDATETIME() );\r\n

--^(\d+)\t([^\s]+)\t([^\t]+)\t([^\t]+)\t(\d+)\t(.+)\r\n$



INSERT INTO [dbo].[Kiosk](KioskID, SecretKey, [KioskName],[Description],[BranchLocationID],[Disabled], CreatedDate) VALUES(1700, '17cedc481859175088a739a89336add8e4d05e08ba882a0356774fc6cafacdc7', 'S/N:11011301851', 'Model: Microsoft Surface Go 2 1926;  OS: Windows 10 P; ',1,0, SYSDATETIME() );

INSERT INTO [dbo].[Kiosk](KioskID, SecretKey, [KioskName],[Description],[BranchLocationID],[Disabled], CreatedDate) VALUES(1701, '7754ce3d90cfc300fc3980b8a792283e1349373ebf1882543dfabfb5c3afdb85', 'S/N:10472701851', 'Model: Microsoft Surface Go 2 1926;  OS: Windows 10 P; ',2,0, SYSDATETIME() );

INSERT INTO [dbo].[Kiosk](KioskID, SecretKey, [KioskName],[Description],[BranchLocationID],[Disabled], CreatedDate) VALUES(1702, '6525123bb51725f4deb763f2b81eef0ec47b03b8925ecd4c33853a9d8b7d58e9', 'S/N:25793201751', 'Model: Microsoft Surface Go 2 1926;  OS: Windows 10 P; ',3,0, SYSDATETIME() );

INSERT INTO [dbo].[Kiosk](KioskID, SecretKey, [KioskName],[Description],[BranchLocationID],[Disabled], CreatedDate) VALUES(1703, '1a098d505a64c53042339f3d8930d10f95158ae25ded0e01bdbd7ab1c04e9c58', 'S/N:25594301751', 'Model: Microsoft Surface Go 2 1926;  OS: Windows 10 P; ',3,0, SYSDATETIME() );

INSERT INTO [dbo].[Kiosk](KioskID, SecretKey, [KioskName],[Description],[BranchLocationID],[Disabled], CreatedDate) VALUES(1704, '615252ff82eb2d281334628e0e1466527e1b7ebaf541d705883c2b9616d5a624', 'S/N:25749101751', 'Model: Microsoft Surface Go 2 1926;  OS: Windows 10 P; ',3,0, SYSDATETIME() );

INSERT INTO [dbo].[Kiosk](KioskID, SecretKey, [KioskName],[Description],[BranchLocationID],[Disabled], CreatedDate) VALUES(1705, '117921ad403f920b701ba138a9a1601cdf644610941e38eefc13d174119ad1c0', 'S/N:25796601751', 'Model: Microsoft Surface Go 2 1926;  OS: Windows 10 P; ',3,0, SYSDATETIME() );




SET IDEnTITY_INSERT dbo.Kiosk OFF
select * from dbo.Kiosk

SET IDEnTITY_INSERT dbo.Kiosk OFF

commit transaction
