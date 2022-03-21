IF NOT EXISTS(SELECT 1 FROM dbo.Screening)
    INSERT INTO Screening(ScreeningID, ScreeningName) VALUES('BHS', 'Behavioral Health Screening');
GO

-- Answer Scale and Options
MERGE INTO dbo.AnswerScale as target
USING (
VALUES
( 1, 'Yes / No'),
( 2, 'PHQ-9'),
( 3, 'PHQ-9 Difficulty'),
( 4, 'HITS'),
(5, 'Drug Of Choice First'),
(6, 'Drug Of Choice Others')
) AS source(AnswerScaleID, Description) ON target.AnswerScaleID = source.AnswerScaleID
WHEN MATCHED THEN  
    UPDATE SET [Description] = source.[Description]
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (AnswerScaleID, Description) VALUES (source.AnswerScaleID, source.Description)  
;
GO


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


-- Section Questions, with Y/N answer
MERGE INTO dbo.ScreeningSection as target
USING ( VALUES
('CIF', 'BHS', 'CIF', 'Contact Information', '', 0),
('DMGR', 'BHS', 'DMGR', 'Patient Demographics', '', 1),
('SIH', 'BHS', 'SIH', 'Smoker in the Home', 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 2),
('TCC', 'BHS', 'TCC', 'Tobacco Use', 'Do you use tobacco?', 3),
('CAGE', 'BHS', 'CAGE', 'Alcohol Use (CAGE)', 'Do you drink alcohol?',4),
('DAST', 'BHS', 'DAST-10', 'Non-Medical Drug Use (DAST-10)', 'Have you used drugs other than those required for medical reasons?',5),
('DOCH', 'BHS', 'DOCH', 'Drug Use', 'What Drug do you USE THE MOST?', 6),

('GAD-7', 'BHS', 'GAD-7', 'Anxiety (GAD-7)', 'How often have you been bothered by the following problems?', 7),
('GAD7A', 'BHS', 'GAD7A', 'Anxiety (GAD-7)', '', 7),

('PHQ-9', 'BHS', 'PHQ-9', 'Depression (PHQ-9)', 'Do you feel down, depressed, or hopeless?', 8),
('PHQ9A', 'BHS', 'PHQ9A', 'Depression (PHQ-9)', '', 8),


('HITS', 'BHS', 'HITS', 'Intimate Partner/Domestic Violence (HITS)', 'Do you feel UNSAFE in your home?', 9),


('BBGS', 'BHS', 'BBGS', 'Problem Gambling (BBGS)', '', 20)


) AS source(ScreeningSectionID, ScreeningID, ScreeningSectionShortName, ScreeningSectionName, QuestionText, OrderIndex)
    ON source.ScreeningSectionID = target.ScreeningSectionID
WHEN MATCHED THEN  
    UPDATE SET ScreeningID = source.ScreeningID, 
        ScreeningSectionShortName = source.ScreeningSectionShortName, 
        ScreeningSectionName = source.ScreeningSectionName,
        QuestionText = source.QuestionText,
        OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (ScreeningSectionID,ScreeningID,ScreeningSectionShortName, ScreeningSectionName,QuestionText, OrderIndex) 
        VALUES (source.ScreeningSectionID, source.ScreeningID, source.ScreeningSectionShortName, source.ScreeningSectionName, source.QuestionText, source.OrderIndex)  
;
GO
---------------------------------------------

MERGE INTO ScreeningSectionQuestion as Target
USING( VALUES
('SIH', 1, NULL, 'Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 1, 1, 0, 10),

('TCC', 4, NULL, 'Do you use tobacco?', 1, 1, 0, 10),
('TCC', 1, NULL, 'Do you use tobacco for ceremony?', 1, 0, 0, 100),
('TCC', 2, NULL, 'Do you smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 1, 0, 0, 100),
('TCC', 3, NULL, 'Do you use smokeless tobacco?', 1, 0, 0, 100),

('CAGE', 5, NULL, 'Do you drink alcohol?', 1, 1, 0, 10),
('CAGE', 1, NULL, 'Have you ever felt you should CUT down on your drinking?', 1, 0, 0, 100),
('CAGE', 2, NULL, 'Have people ANNOYED you by criticizing your drinking?', 1, 0, 0, 100),
('CAGE', 3, NULL, 'Have you ever felt bad or GUILTY about your drinking?', 1, 0, 0, 100),
('CAGE', 4, NULL, 'Have you ever had a drink first thing in the morning to steady your nerves or get rid of a hangover (EYE-OPENER)?', 1, 0, 0, 100),

('DAST', 10, 'Over the LAST 12 MONTHS:', 'Have you used drugs other than those required for medical reasons?', 1, 1, 0, 10),
('DAST', 1, 'Over the LAST 12 MONTHS:', 'Do you abuse more than one drug at a time?', 1, 0, 0, 100),
('DAST', 2, 'Over the LAST 12 MONTHS:', 'Are you always able to stop using drugs when you want to?', 1, 0, 0, 100),
('DAST', 3, 'Over the LAST 12 MONTHS:', 'Have you had “blackouts” or “flashbacks” as a result of drug use?', 1, 0, 0, 100),
('DAST', 4, 'Over the LAST 12 MONTHS:', 'Do you ever feel bad or guilty about your drug use?', 1, 0, 0, 100),
('DAST', 5, 'Over the LAST 12 MONTHS:', 'Does your spouse (or parent) ever complain about your involvement with drugs?', 1, 0, 0, 100),
('DAST', 6, 'Over the LAST 12 MONTHS:', 'Have you neglected your family because of your use of drugs?', 1, 0, 0, 100),
('DAST', 7, 'Over the LAST 12 MONTHS:', 'Have you engaged in illegal activities in order to obtain drugs?', 1, 0, 0, 100),
('DAST', 8, 'Over the LAST 12 MONTHS:', 'Have you ever experienced withdrawal symptoms (felt sick) when you stopped taking drugs?', 1, 0, 0, 100),
('DAST', 9, 'Over the LAST 12 MONTHS:', 'Have you had medical problems as a result of your drug use (e.g., memory loss, hepatitis, convulsions, bleeding)?', 1, 0, 0, 100),

('PHQ-9', 1, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Little interest or pleasure in doing things?', 2, 1, 0, 10),
('PHQ-9', 2, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling down, depressed, or hopeless?', 2, 1, 0, 20),
('PHQ-9', 3, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble falling or staying asleep, or sleeping too much?', 2, 0, 0, 100),
('PHQ-9', 4, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling tired or having little energy?', 2, 0, 0, 100),
('PHQ-9', 5, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Poor appetite or overeating?', 2, 0, 0, 100),
('PHQ-9', 6, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling bad about yourself - or that you are a failure or have let yourself or your family down?', 2, 0, 0, 100),
('PHQ-9', 7, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble concentrating on things, such as reading the newspaper or watching television?', 2, 0, 0, 100),
('PHQ-9', 8, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Moving or speaking so slowly that other people could have noticed. Or the opposite - being so fidgety or restless that you have been moving around a lot more than usual?', 2, 0, 0, 100),
('PHQ-9', 9, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Thoughts that you would be BETTER OFF DEAD or of HURTING YOURSELF in some way?', 2, 0, 0, 100),
('PHQ-9', 10, NULL, 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?', 3, 0, 0, 100),

('PHQ9A', 1, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Little interest or pleasure in doing things?', 2, 1, 0, 10),
('PHQ9A', 2, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling down, depressed, or hopeless?', 2, 1, 0, 20),
('PHQ9A', 3, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble falling or staying asleep, or sleeping too much?', 2, 1, 0, 100),
('PHQ9A', 4, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling tired or having little energy?', 2, 1, 0, 100),
('PHQ9A', 5, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Poor appetite or overeating?', 2, 1, 0, 100),
('PHQ9A', 6, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling bad about yourself - or that you are a failure or have let yourself or your family down?', 2, 1, 0, 100),
('PHQ9A', 7, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble concentrating on things, such as reading the newspaper or watching television?', 2, 1, 0, 100),
('PHQ9A', 8, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Moving or speaking so slowly that other people could have noticed. Or the opposite - being so fidgety or restless that you have been moving around a lot more than usual?', 2, 1, 0, 100),
('PHQ9A', 9, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Thoughts that you would be BETTER OFF DEAD or of HURTING YOURSELF in some way?', 2, 1, 0, 100),
('PHQ9A', 10, NULL, 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?', 3, 1, 0, 100),



('HITS', 5, 'Over the LAST 12 MONTHS:', 'Did a partner, family member, or caregiver hurt, insult, threaten, or scream at you?', 1, 1, 0, 10),
('HITS', 1, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'Physically HURT you?', 4, 0, 0, 100),
('HITS', 2, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'INSULT or talk down to you?', 4, 0, 0, 100),
('HITS', 3, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'THREATEN you with physical harm?', 4, 0, 0, 100),
('HITS', 4, 'Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:', 'SCREAM or curse at you?', 4, 0, 0, 100),


('DOCH', 1, NULL, 'What Drug do you use THE MOST?', 5, 1, 0, 10),
('DOCH', 2, NULL, 'What DRUG do you use SECOND MOST?', 6, 0, 0, 100),
('DOCH', 3, NULL, 'Do you use ANY OTHER DRUG?', 6, 0, 0, 100),

-- Anxiety
('GAD-7', 1, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling ner vous, anxious, or on edge?', 2, 1, 0, 10),
('GAD-7', 2, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Not being able to stop or control worrying?', 2, 1, 0, 20),
('GAD-7', 3, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Worrying too much about different things?', 2, 0, 0, 100),
('GAD-7', 4, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble relaxing?', 2, 0, 0, 100),
('GAD-7', 5, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Being so restless that it is hard to sit still?', 2, 0, 0, 100),
('GAD-7', 6, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Becoming easily annoyed or irritable?', 2, 0, 0, 100),
('GAD-7', 7, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling afraid as if something awful might happen?', 2, 0, 0, 100),
('GAD-7', 8, NULL, 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?', 3, 0, 1, 100),

('GAD7A', 1, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling ner vous, anxious, or on edge?', 2, 1, 0, 10),
('GAD7A', 2, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Not being able to stop or control worrying?', 2, 1, 0, 20),
('GAD7A', 3, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Worrying too much about different things?', 2, 1, 0, 100),
('GAD7A', 4, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Trouble relaxing?', 2, 1, 0, 100),
('GAD7A', 5, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Being so restless that it is hard to sit still?', 2, 1, 0, 100),
('GAD7A', 6, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Becoming easily annoyed or irritable?', 2, 1, 0, 100),
('GAD7A', 7, 'Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems:', 'Feeling afraid as if something awful might happen?', 2, 1, 0, 100),
('GAD7A', 8, NULL, 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?', 3, 0, 1, 100),

-- Problem Gambling
('BBGS', 1, 'In the PAST 12 MONTHS, have you gambled?', '(Examples of gambling include lottery scratchers or draw tickets, casino games, daily fantasy sports, bingo, online poker, horse racing, etc.)?', 1, 1, 0, 10),
('BBGS', 2, 'During the PAST 12 MONTHS:', 'Have you become restless, irritable, or anxious when trying to stop/cut down on gambling?', 1, 0, 0, 100),
('BBGS', 3, 'During the PAST 12 MONTHS:', 'Have you tried to keep your family or friends from knowing how much you gambled?', 1, 0, 0, 100),
('BBGS', 4, 'During the PAST 12 MONTHS:', 'Did you have such financial trouble as a result of your gambling that you had to get help with living expenses from family, friends, or welfare?', 1, 0, 0, 100)


) as Source(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive,OrderIndex)
    ON Target.ScreeningSectionID = Source.ScreeningSectionID AND Target.QuestionID = Source.QuestionID

WHEN MATCHED THEN
    UPDATE SET 
        PreambleText = Source.PreambleText, 
        QuestionText = Source.QuestionText,
        AnswerScaleID = Source.AnswerScaleID,
        IsMainQuestion = Source.IsMainQuestion,
        OnlyWhenPossitive = Source.OnlyWhenPossitive,
        OrderIndex = Source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex)
    VALUES(Source.ScreeningSectionID, Source.QuestionID, Source.PreambleText, Source.QuestionText, Source.AnswerScaleID, Source.IsMainQuestion, Source.OnlyWhenPossitive, Source.OrderIndex)
    ;
GO


---- Insert States
DELETE FROM State;

INSERT INTO State(StateCode, Name, CountryCode)
VALUES
('AK', 'Alaska', 'US'),
('AL', 'Alabama', 'US'),
('AR', 'Arkansas', 'US'),
('AZ', 'Arizona', 'US'),
('CA', 'California', 'US'),
('CO', 'Colorado', 'US'),
('CT', 'Connecticut', 'US'),
('DC', 'District of Columbia', 'US'),
('DE', 'Delaware', 'US'),
('FL', 'Florida', 'US'),
('GA', 'Georgia', 'US'),
('GU', 'Guam', 'US'),
('HI', 'Hawaii', 'US'),
('IA', 'Iowa', 'US'),
('ID', 'Idaho', 'US'),
('IL', 'Illinois', 'US'),
('IN', 'Indiana', 'US'),
('KS', 'Kansas', 'US'),
('KY', 'Kentucky', 'US'),
('LA', 'Louisiana', 'US'),
('MA', 'Massachusetts', 'US'),
('MD', 'Maryland', 'US'),
('ME', 'Maine', 'US'),
('MI', 'Michigan', 'US'),
('MN', 'Minnesota', 'US'),
('MO', 'Missouri', 'US'),
('MS', 'Mississippi', 'US'),
('MT', 'Montana', 'US'),
('NC', 'North Carolina', 'US'),
('ND', 'North Dakota', 'US'),
('NE', 'Nebraska', 'US'),
('NH', 'New Hampshire', 'US'),
('NJ', 'New Jersey', 'US'),
('NM', 'New Mexico', 'US'),
('NV', 'Nevada', 'US'),
('NY', 'New York', 'US'),
('OH', 'Ohio', 'US'),
('OK', 'Oklahoma', 'US'),
('OR', 'Oregon', 'US'),
('PA', 'Pennsylvania', 'US'),
('PR', 'Puerto Rico', 'US'),
('RI', 'Rhode Island', 'US'),
('SC', 'South Carolina', 'US'),
('SD', 'South Dakota', 'US'),
('TN', 'Tennessee', 'US'),
('TX', 'Texas', 'US'),
('UT', 'Utah', 'US'),
('VA', 'Virginia', 'US'),
('VT', 'Vermont', 'US'),
('WA', 'Washington', 'US'),
('WI', 'Wisconsin', 'US'),
('WV', 'West Virginia', 'US'),
('WY', 'Wyoming', 'US');
 
GO



MERGE INTO dbo.ScreeningScoreLevel as target
USING ( VALUES
('TCC', 0, 'NEGATIVE', 'Negative', 'Negative'),
('TCC', 1, 'POSITIVE', 'Positive', 'Positive'),
('SIH', 0, 'NEGATIVE', 'Negative', 'Negative'),
('SIH', 1, 'POSITIVE', 'Positive', 'Positive'),

('CAGE', 0, 'NEGATIVE', 'No problems reported', 'Negative'),
('CAGE', 1, 'Evidence of AT RISK', 'Need for further clinical investigation, including questions on amount, frequency, etc.', 'At Risk'),
('CAGE', 2, 'Evidence of CURRENT PROBLEM', 'Need for further clinical investigation and/or referral as indicated by clinician''s expertise', 'Current Problem'),
('CAGE', 3, 'Evidence of DEPENDENCE until ruled out', 'Evaluate, treat, and/or referral as indicated by clinician''s expertise', 'Dependence'),

('PHQ-9', 0, 'NONE-MINIMAL depression severity', 'No proposed treatment action', 'None-Minimal'), -- No Depression

('PHQ-9', 2, 'MILD depression severity', 'Watchful waiting; repeat PHQ-9 at follow-up', 'Mild'),
('PHQ-9', 3, 'MODERATE depression severity', 'Treatment plan, considering counseling, follow-up and/or pharmacotherapy', 'Moderate'),
('PHQ-9', 4, 'MODERATELY SEVERE depression severity' , 'Active treatment with pharmacotherapy and/or psychotherapy', 'Moderately Severe'),
('PHQ-9', 5, 'SEVERE depression severity', 'Immediate initiation of pharmacotherapy and, if severe impairment or poor response to therapy, expedited referral to a mental health specialist for psychotherapy and/or collaborative management', 'Severe'),

('HITS', 0, 'NEGATIVE', 'No problems reported. Review with patient (if possible)', 'Negative'),
('HITS', 1, 'Evidence of CURRENT PROBLEM', 'Need for immediate investigation and/or referral', 'Current Problem'),

('DAST', 0, 'NEGATIVE', 'No problems reported', 'Negative'),
('DAST', 1, 'LOW LEVEL degree of problem related to drug use', 'Monitor and re-assess at a later date', 'Low'),
('DAST', 2, 'MODERATE LEVEL degree of problem related to drug use', 'Further investigation is required', 'Moderate'),
('DAST', 3, 'SUBSTANTIAL LEVEL degree of problem related to drug use', 'Assessment required', 'Substantial'),
('DAST', 4, 'SEVERE LEVEL degree of problem related to drug use', 'Assessment required', 'Severe'),

('DOCH', 0, 'NEGATIVE', 'Negative', 'Negative'),
('DOCH', 1, 'POSITIVE', 'Positive', 'Positive'),


('GAD-7', 0, 'NONE-MINIMAL anxiety severity', 'No proposed treatment action', 'None-Minimal'),
('GAD-7', 1, 'MILD anxiety severity', 'Provide general feedback, repeat GAD-7 at follow-up', 'Mild'),
('GAD-7', 2, 'MODERATE anxiety severity', 'Further evaluation needed and referral to mental health program', 'Moderate'),
('GAD-7', 3, 'SEVERE anxiety severity', 'Further evaluation needed and referral to mental health program', 'Severe'),

('BBGS', 0, 'NEGATIVE', 'No problems reported', 'Negative'),
('BBGS', 1, 'Evidence of PROBLEM GAMBLING', 'Need for immediate investigation and/or referral', 'Evidence of Problem Gambling')



) AS source (ScreeningSectionID, ScoreLevel, [Name], Indicates, [Label])
    ON target.ScreeningSectionID = source.ScreeningSectionID AND target.ScoreLevel = source.ScoreLevel
WHEN MATCHED THEN
    UPDATE SET 
        [Name] = Source.[Name], 
        Indicates = Source.Indicates,
        Label = Source.Label
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ScreeningSectionID, ScoreLevel, [Name], Indicates, Label)
    VALUES(source.ScreeningSectionID, source.ScoreLevel, source.[Name], source.Indicates, source.Label)
;
GO


---------------------------------------
-- Security Events
---------------------------------------
INSERT INTO SecurityEventCategory(SecurityEventCategoryID, CategoryName)
            VALUES	(1, 'System Security'),
                    (2, 'Accessing patient info'),
                    (3, 'Branch management'),
                    (4, 'Kiosk management');
GO				
insert into SecurityEvent(SecurityEventID, SecurityEventCategoryID, [Description], Enabled) 
values
--system security
(1, 1, 'User was logged into the system',1),
(2, 1, 'Password was changed',1),
(3, 1, 'Security question and/or answer were changed',1),
(4, 1, 'New user was created',1),
(5, 1, 'New account was activated',1),
-- accessing screen results 
(6, 2, 'Behavioral Health Screening Report was read',1),
(7, 2, 'Behavioral Health Screening Report was printed',1),
(12, 2, 'Behavioral Health Screening Report was removed',1),
(13, 2, 'Patient contact information was changed',1),	  
(14, 2, 'Behavioral Health Screening Report was exported',1),
-- Branch location mgmt
(8, 3, 'New branch location was created',1),
(9, 3, 'Branch location was removed',1),
-- Kiosk mgmt
(10, 4, 'New kiosk was registered',1),
(11, 4, 'Kiosk was removed',1),
(12, 2, 'Behavioral Health Screening Report was removed');
GO	
              


IF  EXISTS(SELECT 1 FROM dbo.ScreeningProfile)
    SET NOEXEC ON

GO

SET IDENTITY_INSERT dbo.ScreeningProfile ON

INSERT INTO dbo.ScreeningProfile (ID, Name, Description, LastModifiedDateUTC)
VALUES ( 1, 'Default', 'Default screening profile', GETUTCDATE());

SET IDENTITY_INSERT dbo.ScreeningProfile OFF

---- Screening Profile Frequency
MERGE INTO dbo.ScreeningProfileFrequency as target
USING ( VALUES
(1, 'CIF', 0, GETUTCDATE()),
(1,'SIH', 0, GETUTCDATE()),
(1, 'TCC', 0, GETUTCDATE()),
(1, 'CAGE', 0, GETUTCDATE()),
(1, 'DAST', 1200, GETUTCDATE()),
(1, 'PHQ-9', 0, GETUTCDATE()),
(1, 'HITS', 0, GETUTCDATE()),
(1, 'DMGR', 0, GETUTCDATE()),
(1, 'GAD-7', 0, GETUTCDATE()),
(1, 'BBGS', 1200, GETUTCDATE())

) AS source (ScreeningProfileID, ScreeningSectionID, Frequency, LastModifiedDateUTC)
    ON target.ScreeningProfileID = source.ScreeningProfileID AND target.ScreeningSectionID = source.ScreeningSectionID
WHEN MATCHED THEN
    UPDATE SET 
        Frequency = Source.Frequency, 
        LastModifiedDateUTC = Source.LastModifiedDateUTC
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ScreeningProfileID, ScreeningSectionID, Frequency, LastModifiedDateUTC)
    VALUES(source.ScreeningProfileID, source.ScreeningSectionID, source.Frequency, source.LastModifiedDateUTC)
;
GO
              
