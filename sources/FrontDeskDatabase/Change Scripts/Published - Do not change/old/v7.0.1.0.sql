

IF( NOT EXISTS(SELECT 1 FROM dbo.[LookupValuesDeleteLog] WHERE [Name] = 'Agua Caliente, California'))
BEGIN 

INSERT INTO [dbo].[LookupValuesDeleteLog] ([TableName],[Name]) VALUES
('Tribe', 'Agua Caliente, California' ),
('Tribe', 'Cortina Indian Rancheria, California'),
('Tribe', 'Death Valley Timbi-sha Shoshone Tribe, California'),
('Tribe', 'Paiute Indian Tribe (Cedar, Kanosh, Koosharem, Indian Peaks, and Shivwits), Utah')
;
END
GO

DELETE FROM dbo.Tribe
WHERE [Value] IN (
'Agua Caliente, California', 
'Cortina Indian Rancheria, California', 
'Death Valley Timbi-sha Shoshone Tribe, California',
'Paiute Indian Tribe (Cedar, Kanosh, Koosharem, Indian Peaks, and Shivwits), Utah'
);



IF( (SELECT COUNT(*) FROM dbo.Tribe) < 500)
BEGIN 
DECLARE @updateDate datetime = GETUTCDATE();
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Agua Caliente Band of Cahuilla Indians, California', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Chickahominy Indian Tribe, Virginia', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Chickahominy Indian Tribe—Eastern Division, Virginia', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Cortina Indian Rancheria, California', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Death Valley Timbisha Shoshone Tribe, California', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Kletsel Dehe Band of Wintun Indians, California', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Monacan Indian Nation, Virginia', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Nansemond Indian Nation, Virginia', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Paiute Indian Tribe of Utah (Cedar, Kanosh, Koosharem, Indian Peaks, and Shivwits), Utah', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Rappahannock Tribe, Inc., Virginia', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Upper Mattaponi Tribe, Virginia', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Karluk', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kiana', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kipnuk', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kivalina', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kluti Kaah (Copper Center)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kobuk', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kongiganak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kotzebue', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Koyuk', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kwigillingok', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Kwinhagak (Quinhagak)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Larsen Bay', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Marshall (Fortuna Ledge)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Mary’s Igloo', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Mekoryuk', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Minto', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Nanwalek (English Bay)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Napaimute', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Napakiak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Napaskiak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Nelson Lagoon', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Nightmute', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Nikolski', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Noatak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Nuiqsut (Nooiksut)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Nunam Iqua', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Nunapitchuk', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Ouzinkie', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Paimiut', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Perryville', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Pilot Point', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Pitka''s Point', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Point Hope', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Point Lay', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Port Graham', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Port Heiden', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Port Lions', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Ruby', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Saint Michael', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Savoonga', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Scammon Bay', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Selawik', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Shaktoolik', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Shishmaref', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Shungnak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Stevens', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Tanacross', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Tanana', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Tatitlek', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Tazlina', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Teller', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Tetlin', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Tuntutuliak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Tununak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Tyonek', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Unalakleet', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Unga', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Venetie Tribal Government (Arctic Village and Village of Venetie)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of Wales', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Native Village of White Mountain', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Nenana Native Association', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('New Koliganek Village Council', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('New Stuyahok Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Newhalen Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Newtok Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Nikolai Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Ninilchik Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Nome Eskimo Community', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Nondalton Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Noorvik Native Community', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Northway Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Nulato Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Nunakauyarmiut Tribe', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Organized Village of Grayling (Holikachuk)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Organized Village of Kake', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Organized Village of Kasaan', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Organized Village of Kwethluk', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Organized Village of Saxman', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Orutsararmiut Traditional Native Council (Bethel)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Oscarville Traditional Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Pauloff Harbor Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Pedro Bay Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Petersburg Indian Association', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Pilot Station Traditional Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Platinum Traditional Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Portage Creek Village (Ohgsenakale)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Pribilof Islands Aleut Communities of St. Paul & St. George Islands', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Qagan Tayagungin Tribe of Sand Point Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Qawalangin Tribe of Unalaska', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Rampart Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Saint George Island (Pribilof Islands Aleut Communities of St. Paul & St. George Islands)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Saint Paul Island (Pribilof Islands Aleut Communities of St. Paul & St. George Islands)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Salamatof Tribe', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Seldovia Village Tribe', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Shageluk Native Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Sitka Tribe of Alaska', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Skagway Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('South Naknek Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Stebbins Community Association', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Sun''aq Tribe of Kodiak (Shoonaq'' Tribe of Kodiak)', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Takotna Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Tangirnaq Native Village (Lesnoi Village (aka Woody Island))', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Telida Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Traditional Village of Togiak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Tuluksak Native Community', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Twin Hills Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Ugashik Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Umkumiut Native Village', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Alakanuk', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Anaktuvuk Pass', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Aniak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Atmautluak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Bill Moore''s Slough', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Chefornak', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Clarks Point', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Crooked Creek', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Dot Lake', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Iliamna', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Kalskag', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Kaltag', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Kotlik', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Lower Kalskag', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Ohogamiut', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Red Devil', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Salamatoff', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Sleetmute', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Solomon', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Stony River', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Venetie', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Village of Wainwright', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Wrangell Cooperative Association', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Yakutat Tlingit Tribe', @updateDate );
INSERT INTO Tribe ([Value], LastModifiedDateUTC ) VALUES('Yupiit of Andreafski', @updateDate );


END

---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.1.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.1.0');