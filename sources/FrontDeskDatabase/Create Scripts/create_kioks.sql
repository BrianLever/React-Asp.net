/*
reg expressions:
search: ^([^\t]+)\t([^\t]+)\t([^\t]+)\t([^\t]+)\t([^\t]+)\t([^\t]+)\t([^\t]+)\t([^\t]+)\s?\r\n
replace: ('S\N: $6', '$7\\r\\n$3\\r\\n$4', GETUTCDATE(), NULL, $8, 0, '$2'),\r\n
*/

DBCC CHECKIDENT (Kiosk, RESEED, 1499);

INSERT INTO [dbo].[Kiosk]([KioskName], [Description],[CreatedDate],[LastActivityDate],[BranchLocationID],[Disabled],[SecretKey])
     VALUES

('S\N: 7943501551', 'DESKTOP-PBF7HNM\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 1, 0, 'LWXrWrPd2v4RZaen-ifRTCB7yWbwI#uNUlvhp2Wpd-dwCd8bcFyv#huIVY28QeUC'),
('S\N: 7924601551', 'DESKTOP-9DVBPUN\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 1, 0, 'JzwK#SEsSKNTPzLNEgMMJEfJnW6hHuKZdbjvxTyg5Rccc8Ur6vXUngfEUnjgVimm'),
('S\N: 4781101551', 'DESKTOP-G6R2JU3\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 1, 0, '1RqWXmVg1uGkBE5aPd2lhd2RqFZeFHXMWKlagPoZla2k1vAsCll68fqaYYrSN-bw'),
('S\N: 17840101151', 'DESKTOP-P3D4GS2\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 2, 0, 'oyL4£zGefGboM1WKWBbf7ZOD4zBByPufi£l6QylAPudLjK7nFdIfqcQPkHAByFuP'),
('S\N: 8320101551', 'DESKTOP-6RPM37A\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 2, 0, 'QTbkl24qEfB@zr-1un@zlCXVYE47qTlBTpUWW2xpA5UmPGsjmPSxZ5OO4GLh75Aw'),
('S\N: 8028301551', 'DESKTOP-2IRFU7Q\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 3, 0, '£a8QOowq7LRpojnjenuPUlk7pdeXgjTP8q5cHvmm4c5SVYzuUOgxnsUP-AnwJZJa'),
('S\N: 8183101551', 'DESKTOP-UA8N8BD\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 3, 0, 'hENyDUW4qrHMgUEx366rE8@-rJUHwl£DUCRM£c3dzI7pFsIy6c51uHuhQWF2AOKF'),
('S\N: 17002401151', 'DESKTOP-FJOI3P7\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 4, 0, 'pQ@vFZlTIYXJuUe#QEjg3fapgGLEcETrDO8FckwoPIYcD1Hr7Oivi3SJ-KO7Og-D'),
('S\N: 4978701551', 'DESKTOP-0VURU4S\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 4, 0, '£CnDTlwaxkbCieSrMhLj5Yu3psXuk5zZ6#zCzjGiIz2r@TAFOKz2T3-dWrZmOC-@'),
('S\N: 5136601551', 'DESKTOP-SS0DP5D\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 5, 0, 'QYzFLdhAN5UFVRJT2@gJCIWC@CJr2q£Om2s7OmNR3DOwsGR2iGqf4v4IBwQJT@Hj'),
('S\N: 8069501551', 'DESKTOP-INO0MH5\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 5, 0, 'bhp-OmQvTVYi1Waz1LQv1jZMuZxwzwdD1£zD8Gx7Q7bd24YOrCw86oC#HcL£@g5i'),
('S\N: 4359701551', 'DESKTOP-GGQ3RSH\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 6, 0, '#ldAclGHJSj1ABbmiDjuHxwGbAsgC45lo4L@u8d7BEBGpNIWxfP1£7WG£Xlc1mFO'),
('S\N: 8370301551', 'DESKTOP-GNOK2UJ\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 6, 0, 'TdGEpDWXcapjr48£lSe@kidxUS61N1BfgvmI8yPEn85318vWIAlp#oNwAUwXFdJk'),
('S\N: 4449701551', 'DESKTOP-BVM5TMS\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 7, 0, 'XaI-qTcDf#egeLZRF£ob4HZuUi#8xB#JDwyw4IcgoKKuphYex7CX6jfQAdhHccSe'),
('S\N: 16879401151', 'DESKTOP-4KGEALT\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 8, 0, 'PXquNZ@Auba@o@8j#NgIY6Zikr@r@sGmWozMxBrSK65qVKjYu5XnGWZR7IHhkIMA'),
('S\N: 8221101551', 'DESKTOP-GAJ99DS\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 9, 0, 'ehTB2YN7Pf@Fhog1@x6BP8TQl£UJvVeqIPAwfvQ£mf@87KkGmOlU1yzeOCcEEDO2'),
('S\N: 8357501551', 'DESKTOP-A85O2NC\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 10, 0, 'uNyFcFEYXaEgaoQs#VhGWErP85CuuNXQEprTMmT-Qk3ia6ayVYW5XVXmgiFgCZsJ'),
('S\N: 17057101151', 'DESKTOP-MD9OUSJ\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 11, 0, 'isY3£EsHE@QEGFqImBMEjHXeKQVwFa2uxHMwpHe7pCkLZqG4k-#QUh-aW3vkv5Bq'),
('S\N: 8097401551', 'DESKTOP-023R13L\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 12, 0, 'Tj-XjROiMDWoOVugvSFSC8#epmbhpLJoRL86#qmRCsJQUKko3MabvyoUrXs5-r2B'),
('S\N: 4524601551', 'DESKTOP-4HVAP0T\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 13, 0, 'WGfJLJIDBfYhnw-Exe6SrjNoeGluuyRpME1BioM-PS8PQ26GOG@ZlNSz1#uEjZYP'),
('S\N: 17819201151', 'DESKTOP-LB0U38O\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 14, 0, 'feqUFuiwLOZRMDkjKAYbxumS31r7a27vYHcMvLP6IYs£d1Y#IOIOB-F@#YZM7kWR'),
('S\N: 8093101551', 'DESKTOP-L5CED1T\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 15, 0, 'GcZZSTWR2AS3ae3h8vnxUMgGm4Yo1JDsGzem4YRxRzGgM34UwFDFLFa25TyX4uYw'),
('S\N: 7938101551', 'DESKTOP-IPEO8NU\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 15, 0, 'Woi1AUXaa-Wo3XO£g4uG33Q3T4iCH#MsUVqP8MsM1fjxWX@Y-qVb#wxcXolFxVTi'),
('S\N: 17783201151', 'DESKTOP-HPNKTDJ\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 16, 0, 'bHO54lh£WExvnsKkxUx£nZZFIgQriUY-RpQlIURgnZn@H#1UPpRh-BRCsJzbV@SC'),
('S\N: 17942501151', 'DESKTOP-P5FI2C8\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 17, 0, 'gLSzrw5mEQmhogC8wSHNjis@w3lICWbBUFykUyHlk#mIXIzMGBS2H8r6deDxLzku'),
('S\N: 8358401551', 'DESKTOP-M6G1SU5\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 17, 0, 'iJ1w-#LwEo74bTf@NsmM6Tjjx@-ox8Do5mfWbNLOo41IPpR-rKb6x@uPmU7CBj7F'),
('S\N: 17845301151', 'DESKTOP-HA589QE\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 18, 0, 'd8Udj8UN3lbgq1@JjPBVAfIskLMzDOMFin2CvdhweUZZOhju-KLMMhf7-n2YygKr'),
('S\N: 17906501151', 'DESKTOP-IQOK7O0\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 19, 0, 'DLPBzqJMImLDbH8esCzC@YlVH7SV1DBN1ajCDJbsW8hAPgiG-JnekVGul22@X@TE'),
('S\N: 8148701551', 'DESKTOP-5N82QS5\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 20, 0, 'u6I@hlCjRfM-mfOgs4d8D6fD6PdiKMJNFMQwyNec1Cz5d6Uor2O#VG@xdF-SIgYF'),
('S\N: 17879301151', 'DESKTOP-99JHP02\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 21, 0, 'A871qZuQqRcgas6DIhsg£Bd@-LCohnrvYFkmrUQd5IShYwqxFkN3MLe7iJCfXFvD'),
('S\N: 4621101551', 'DESKTOP-32K4V7B\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 21, 0, 'l1rfxWCpp£2cpzShIEJ27hIZm#dePaPpL@AaA£5NybLImRl@rOKyx5yEwS121jaO'),
('S\N: 4754101551', 'DESKTOP-VI0QC5B\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 22, 0, '7KUoXkl8FB@uqa8#mTULlhevJucfM4MFs7£T8wbd54rZny1YUGiAbTHculfuHGyJ'),
('S\N: 17899101151', 'DESKTOP-UCOFVTJ\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 23, 0, 'HIxhpqjXRkT3uMI#£R£kcdk2cIB8DQg72GUuQ#ojmZG5-cym£FWQGQBqk1EgpjTA'),
('S\N: 14230401751', 'DESKTOP-5BH86A9\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 23, 0, 'bq3ULVsqGXav4IL2X1phyY43W8su8C5ns1£7ZHPAG7LTZXMu5xxijxLDv86Mo7uD'),
('S\N: 4869101551', 'DESKTOP-FIDUCMD\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 24, 0, 'QqfmpvRSW8TqFmvEyBy8£3hvJkalbFfGmOjQ£cxSjib66Fq-OlMaIz@VIvdFUu-Q'),
('S\N: 17839701151', 'DESKTOP-85GFBBE\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 24, 0, 'Vycf@oIbxbRPVujXevCZK£kNCwH8FSfJ#DizkG1ahrmccxNOORVLscFfOPcDgIWe'),
('S\N: 4850401551', 'DESKTOP-FHFNV95\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 25, 0, 'oGQT-7oNGZagl1s6umWjMPb56ZHconvfIxR6QeuK1KJ@SkjRyKp2La1YuTWd#Mkf'),
('S\N: 4690201551', 'DESKTOP-03LBEBE\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 25, 0, 'fWh£DzuD#NyHs-@EGun@DmJ3EGa@xLjfJlqCCyQ@ihLXbnh4q5HTzAi1Mmx5Z#Su'),
('S\N: 14110701751', 'DESKTOP-7CTO21F\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 26, 0, 'rZ5HDq7Qmxh8ccmBiyu4£gYD6sZD68Enxw54gQF7KyUwfFrrdE4uOYjMA1JVR4Sn'),
('S\N: 17963201151', 'DESKTOP-3EP48VM\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 26, 0, 'G-6poDfPE5FqLjRxJlAV@JjkK4hDERfySMie3EH2c3mdKHd#T3Rfx3o5SYfricKz'),
('S\N: 21615601551', 'DESKTOP-MIG3F9U\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 27, 0, 'ol2CxxLzfugANlSw7Xjsx2y8iHXZ#M@hTvpkaI#nRfJu2wjCOpmv6-@uz3gpax4T'),
('S\N: 4585601551', 'DESKTOP-AF925MP\\r\\nMS Surface Go\\r\\nWindows 10 P 1909', GETUTCDATE(), NULL, 28, 0, 'Md@lpYMw@uIYnPwjqerukYMQYV3G5s5juU#8CfrB6UJhu6QLRneexJ8zRjMUeGzH')
;