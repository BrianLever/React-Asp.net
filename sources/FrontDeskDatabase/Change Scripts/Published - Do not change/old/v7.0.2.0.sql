
MERGE INTO dbo.AnswerScaleOption as target
USING (
VALUES
( 1, 1, 'Yes', 1),
( 2, 1, 'No', 0),
( 3, 2, 'Not at all', 0),
( 4, 2, 'Several days', 1),
( 5, 2, 'More than half the days', 2),
( 6, 2, 'Nearly every day', 3),
( 7, 3, 'Not difficult at all', 0),
( 8, 3, 'Somewhat difficult', 1),
( 9, 3, 'Very difficult', 2),
( 10, 3, 'Extremely difficult', 3),
( 11, 4, 'Never', 1),
( 12, 4, 'Rarely', 2),
( 13, 4, 'Sometimes', 3),
( 14, 4, 'Fairly Often', 4),
( 15, 4, 'Frequently', 5),

(20, 6, '(None) Don’t Use Any Other Drugs', 0),

(21, 5, 'Methamphetamine', 2),
(22, 6, 'Methamphetamine', 2),

(23, 5, 'Other Amphetamines', 3),
(24, 6, 'Other Amphetamines', 3),

(25, 5, 'Marijuana/Cannabis/Wax/Hashish', 1),
(26, 6, 'Marijuana/Cannabis/Wax/Hashish', 1),

(27, 5, 'Opioid/Medication', 6),
(28, 6, 'Opioid/Medication', 6),

(29, 5, 'Opioid/Heroin', 5),
(30, 6, 'Opioid/Heroin', 5),

(31, 5, 'Benzodiazepines', 4),
(32, 6, 'Benzodiazepines', 4),

(33, 5, 'Cocaine/Crack', 7),
(34, 6, 'Cocaine/Crack', 7),

(35, 5, 'Hallucinogens/Psychedelics', 8),
(36, 6, 'Hallucinogens/Psychedelics', 8),

(37, 5, 'Sedatives/Hypnotics/Non-Benzo Tranquilizers', 9),
(38, 6, 'Sedatives/Hypnotics/Non-Benzo Tranquilizers', 9),

(39, 5, 'Inhalants', 10),
(40, 6, 'Inhalants', 10),

(41, 5, 'Barbiturates/Downers', 11),
(42, 6, 'Barbiturates/Downers', 11),

(43, 5, 'PCP/Ketamine/GHB/Designer Drugs', 12),
(44, 6, 'PCP/Ketamine/GHB/Designer Drugs', 12),

(45, 5, 'Other Stimulants', 13),
(46, 6, 'Other Stimulants', 13),

(47, 5, 'Other', 14),
(48, 6, 'Other', 14)
) AS source(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
    ON source.AnswerScaleOptionID = target.AnswerScaleOptionID
WHEN MATCHED THEN  
    UPDATE SET AnswerScaleID = source.AnswerScaleID, OptionText = source.OptionText, OptionValue = source.OptionValue
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue) 
        VALUES (source.AnswerScaleOptionID, source.AnswerScaleID, source.OptionText, source.OptionValue)  
;
GO


---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.2.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.2.0');