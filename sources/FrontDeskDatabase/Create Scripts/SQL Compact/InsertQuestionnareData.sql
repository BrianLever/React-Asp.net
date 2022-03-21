-- Initialize Screening questiona
delete from ScreeningSectionQuestion;
GO
delete from ScreeningSection;
GO
delete from Screening;
GO
;
GO

INSERT INTO Screening(ScreeningID, ScreeningName) VALUES('BHS', 'Behavioral Health Screening');
GO

DELETE FROM AnswerScaleOption;
GO
DELETE FROM AnswerScale;
GO

--- Answer Scale and Options
INSERT INTO AnswerScale(AnswerScaleID, Description)
VALUES( 1, 'Yes / No');
GO
INSERT INTO AnswerScale(AnswerScaleID, Description)
VALUES( 2, 'PHQ-9');
GO
INSERT INTO AnswerScale(AnswerScaleID, Description)
VALUES( 3, 'PHQ-9 Difficulty');
GO
INSERT INTO AnswerScale(AnswerScaleID, Description)
VALUES( 4, 'HITS');
GO
;
GO

INSERT INTO AnswerScale(AnswerScaleID, Description)
VALUES(5, 'Drug Of Choice First');
GO

INSERT INTO AnswerScale(AnswerScaleID, Description)
VALUES(6, 'Drug Of Choice Others');


INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 1, 1, 'Yes', 1);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 2, 1, 'No', 0);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 3, 2, 'Not at all', 0);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 4, 2, 'Several days', 1);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 5, 2, 'More than half the days', 2);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 6, 2, 'Nearly every day', 3);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 7, 3, 'Not difficult at all', 0);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 8, 3, 'Somewhat difficult', 1);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 9, 3, 'Very difficult', 2);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 10, 3, 'Extremely difficult', 3);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 11, 4, 'Never', 1);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 12, 4, 'Rarely', 2);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 13, 4, 'Sometimes', 3);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 14, 4, 'Fairly Often', 4);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES( 15, 4, 'Frequently', 5);
GO



INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (20, 6, '(None) Don’t Use Any Other Drugs', 0);
GO


INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (21, 5, 'Methamphetamine', 2);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (22, 6, 'Methamphetamine', 2);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (23, 5, 'Other Amphetamines', 3);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (24, 6, 'Other Amphetamines', 3);
GO


INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (25, 5, 'Marijuana/Cannabis/Wax/Hashish', 1);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (26, 6, 'Marijuana/Cannabis/Wax/Hashish', 1);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (27, 5, 'Opioid/Medication', 6);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (28, 6, 'Opioid/Medication', 6);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (29, 5, 'Opioid/Heroin', 5);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (30, 6, 'Opioid/Heroin', 5);
GO


INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (31, 5, 'Benzodiazepines', 4);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (32, 6, 'Benzodiazepines', 4);
GO


INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (33, 5, 'Cocaine/Crack', 7);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (34, 6, 'Cocaine/Crack', 7);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (35, 5, 'Hallucinogens/Psychedelics', 8);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (36, 6, 'Hallucinogens/Psychedelics', 8);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (37, 5, 'Sedatives/Hypnotics/Non-Benzo Tranquilizers', 9);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (38, 6, 'Sedatives/Hypnotics/Non-Benzo Tranquilizers', 9);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (39, 5, 'Inhalants', 10);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (40, 6, 'Inhalants', 10);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (41, 5, 'Barbiturates/Downers', 11);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (42, 6, 'Barbiturates/Downers', 11);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (43, 5, 'PCP/Ketamine/GHB/Designer Drugs', 12);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (44, 6, 'PCP/Ketamine/GHB/Designer Drugs', 12);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (45, 5, 'Other Stimulants', 13);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (46, 6, 'Other Stimulants', 13);
GO

INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (47, 5, 'Other', 14);
GO
INSERT INTO AnswerScaleOption(AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue)
VALUES (48, 6, 'Other', 14);
GO


DELETE FROM ScreeningSection;
GO
-- Section Qiestions, with Y/N answer
INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('SIH', 'BHS', 'Smoker in the Home', 'Does anyone in the home smoke tobacco\n(such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 0);
GO

INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('TCC', 'BHS', 'Tobacco Cessation Counseling', 'Do you use tobacco?', 1);
GO
INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('CAGE', 'BHS', 'Alcohol Use (CAGE)', 'Do you drink alcohol?',2);
GO

INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES ('DAST', 'BHS', 'Non-Medical Drug Use (DAST-10)', 'Have you used drugs other than those required for medical reasons?',3);
GO

INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('DOCH', 'BHS', 'Drug Use', 'What <b>DRUG</b> do you use <b>THE MOST</b>?', 4)



INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('GAD-7', 'BHS', 'Anxiety (GAD-7)', 'How often have you been bothered by the following problems?', 5);
GO

INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('GAD7A', 'BHS', 'Anxiety (GAD-7)', 'How often have you been bothered by the following problems?', 5);
GO




INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('PHQ-9', 'BHS', 'Depression (PHQ-9)', 'Do you feel down, depressed, or hopeless?', 6);
GO

INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('PHQ9A', 'BHS', 'Depression (PHQ-9)', 'Do you feel down, depressed, or hopeless?', 6);
GO

INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES('HITS', 'BHS', 'Intimate Partner/Domestic Violence (HITS)', 'Do you feel <b>UNSAFE</b> in your home?', 7);
GO


INSERT INTO ScreeningSection(ScreeningSectionID,ScreeningID,ScreeningSectionName,QuestionText, OrderIndex)
VALUES ('BBGS', 'BHS', 'Problem Gambling (BBGS)', '',20);
GO

;
GO

delete from ScreeningSectionQuestion;
GO

INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex) VALUES
('SIH', 1, NULL, 'Does anyone in the home smoke tobacco (such as cigarettes,\n cigars, pipes, electronic nicotine delivery devices, etc.)?', 1, 1, 10);
GO


INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex) VALUES
('TCC', 4, NULL, 'Do you use tobacco?', 1, 1, 10);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex) VALUES
('TCC', 1, NULL, 'Do you use tobacco for ceremony?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex) VALUES
('TCC', 2, NULL, 'Do you smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex) VALUES
('TCC', 3, NULL, 'Do you use smokeless tobacco?', 1, 0, 100);
GO




INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('CAGE', 5, NULL, 'Do you drink alcohol?', 1, 1, 10);
GO

INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('CAGE', 1, NULL, 'Have you ever felt you should <b>CUT</b> down on your drinking?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('CAGE', 2, NULL, 'Have people <b>ANNOYED</b> you by criticizing your drinking?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('CAGE', 3, NULL, 'Have you ever felt bad or <b>GUILTY</b> about your drinking?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('CAGE', 4, NULL, 'Have you ever had a drink first thing in the morning to steady your \nnerves or get rid of a hangover (<b>EYE-OPENER</b>)?', 1, 0, 100);
GO


INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 10, 'Over the <b>LAST 12 MONTHS</b>:', 'Have you used drugs other than those required for medical reasons?', 1, 1, 10);
GO

INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 1, 'Over the <b>LAST 12 MONTHS</b>:', 'Do you abuse more than one drug at a time?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 2, 'Over the <b>LAST 12 MONTHS</b>:', 'Are you always able to stop using drugs when you want to?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 3, 'Over the <b>LAST 12 MONTHS</b>:', 'Have you had “blackouts” or “flashbacks” as a result of drug use?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 4, 'Over the <b>LAST 12 MONTHS</b>:', 'Do you ever feel bad or guilty about your drug use?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 5, 'Over the <b>LAST 12 MONTHS</b>:', 'Does your spouse (or parent) ever complain\nabout your involvement with drugs?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 6, 'Over the <b>LAST 12 MONTHS</b>:', 'Have you neglected your family because of your use of drugs?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 7, 'Over the <b>LAST 12 MONTHS</b>:', 'Have you engaged in illegal activities in order to obtain drugs?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 8, 'Over the <b>LAST 12 MONTHS</b>:', 'Have you ever experienced withdrawal symptoms (felt sick)\nwhen you stopped taking drugs?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DAST', 9, 'Over the <b>LAST 12 MONTHS</b>:', 'Have you had medical problems as a result of your drug use\n(e.g., memory loss, hepatitis, convulsions, bleeding)?', 1, 0, 100);
GO



INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 1, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Little interest or pleasure in doing things?', 2, 1, 10);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 2, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling down, depressed, or hopeless?', 2, 1, 20);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 3, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Trouble falling or staying asleep, or sleeping too much?', 2, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 4, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling tired or having little energy?', 2, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 5, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Poor appetite or overeating?', 2, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 6, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling bad about yourself - or that you are a failure or\nhave let yourself or your family down?', 2, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 7, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Trouble concentrating on things, such as reading\n the newspaper or watching television?', 2, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 8, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Moving or speaking so slowly that other people could\n have noticed? Or the opposite - being so fidgety or restless\nthat you have been moving around a lot more than usual?', 2, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 9, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Thoughts that you would be better off dead or\nof hurting yourself in some way?', 2, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ-9', 10, NULL, 'If you checked off <b>ANY</b> problems, how <b>DIFFICULT</b> have these \nproblems made it for you to do your work, take care of things\nat home, or get along with other people?', 3, 0, 100);
GO



INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 1, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Little interest or pleasure in doing things?', 2, 1, 10);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 2, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling down, depressed, or hopeless?', 2, 1, 20);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 3, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Trouble falling or staying asleep, or sleeping too much?', 2, 1, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 4, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling tired or having little energy?', 2, 1, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 5, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Poor appetite or overeating?', 2, 1, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 6, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling bad about yourself - or that you are a failure or\nhave let yourself or your family down?', 2, 1, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 7, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Trouble concentrating on things, such as reading\n the newspaper or watching television?', 2, 1, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 8, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Moving or speaking so slowly that other people could\n have noticed? Or the opposite - being so fidgety or restless\nthat you have been moving around a lot more than usual?', 2, 1, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 9, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Thoughts that you would be better off dead or\nof hurting yourself in some way?', 2, 1, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('PHQ9A', 10, NULL, 'If you checked off <b>ANY</b> problems, how <b>DIFFICULT</b> have these \nproblems made it for you to do your work, take care of things\nat home, or get along with other people?', 3, 1, 100);
GO



INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('HITS', 5, 'Over the LAST 12 MONTHS:', 'Did a partner, family member, or caregiver\nhurt, insult, threaten, or scream at you?', 1, 1, 10);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('HITS', 1, 'Over the <b>LAST 12 MONTHS</b>, how often did your \npartner, family member, or caregiver:', 'Physically <b>HURT</b> you?', 4, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('HITS', 2, 'Over the <b>LAST 12 MONTHS</b>, how often did your \npartner, family member, or caregiver:', '<b>INSULT</b> or talk down to you?', 4, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('HITS', 3, 'Over the <b>LAST 12 MONTHS</b>, how often did your \npartner, family member, or caregiver:', '<b>THREATEN</b> you with physical harm?', 4, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES('HITS', 4, 'Over the <b>LAST 12 MONTHS</b>, how often did your \npartner, family member, or caregiver:', '<b>SCREAM</b> or curse at you?', 4, 0, 100);
GO
;
GO


INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DOCH', 1, NULL, 'What <b>DRUG</b> do you use <b>THE MOST</b>?', 5, 1, 10);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DOCH', 2, NULL, 'What <b>DRUG</b> do you use <b>SECOND MOST</b>?', 6, 0, 10);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('DOCH', 3, NULL, 'Do you use <b>ANY OTHER DRUG</b>?', 6, 0, 10);
GO


-- Anxiety
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD-7', 1, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling nervous, anxious, or on edge?', 2, 1, 0, 10);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD-7', 2, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Not being able to stop or control worrying?', 2, 1, 0, 20);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD-7', 3, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Worrying too much about different things?', 2, 0, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD-7', 4, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Trouble relaxing?', 2, 0, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD-7', 5, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Being so restless that it is hard to sit still?', 2, 0, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD-7', 6, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Becoming easily annoyed or irritable?', 2, 0, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD-7', 7, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling afraid as if something awful might happen?', 2, 0, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD-7', 8, NULL, 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?', 3, 0, 1, 100);
GO

INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD7A', 1, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling nervous, anxious, or on edge?', 2, 1, 0, 10);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD7A', 2, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Not being able to stop or control worrying?', 2, 1, 0, 20);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD7A', 3, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Worrying too much about different things?', 2, 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD7A', 4, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Trouble relaxing?', 2, 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD7A', 5, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Being so restless that it is hard to sit still?', 2, 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD7A', 6, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Becoming easily annoyed or irritable?', 2, 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD7A', 7, 'Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems:', 'Feeling afraid as if something awful might happen?', 2, 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OnlyWhenPossitive, OrderIndex) VALUES
('GAD7A', 8, NULL, 'If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?', 3, 0, 1, 100)
GO

INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('BBGS', 1, 'In the <b>PAST 12 MONTHS</b>, have you gambled?', '(Examples of gambling include lottery scratchers or\ndraw tickets, casino games, daily fantasy sports, bingo,\nonline poker, horse racing, etc.)', 1, 1, 10);
GO

INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('BBGS', 2, 'During the <b>PAST 12 MONTHS</b>:', 'Have you become restless, irritable, or anxious\nwhen trying to stop/cut down on gambling?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('BBGS', 3, 'During the <b>PAST 12 MONTHS</b>:', 'Have you tried to keep your family or friends\nfrom knowing how much you gambled?', 1, 0, 100);
GO
INSERT INTO ScreeningSectionQuestion(ScreeningSectionID, QuestionID, PreambleText, QuestionText, AnswerScaleID, IsMainQuestion, OrderIndex)
VALUES ('BBGS', 4, 'During the <b>PAST 12 MONTHS</b>:', 'Did you have such financial trouble as a result of your\ngambling that you had to get help with living expenses\nfrom family, friends, or welfare?', 1, 0, 100);
GO



---- Insert States
DELETE FROM State;
GO

INSERT INTO State(StateCode, Name, CountryCode)
VALUES('AK', 'Alaska', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('AL', 'Alabama', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('AR', 'Arkansas', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('AZ', 'Arizona', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('CA', 'California', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('CO', 'Colorado', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('CT', 'Connecticut', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('DC', 'District of Columbia', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('DE', 'Delaware', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('FL', 'Florida', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('GA', 'Georgia', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('GU', 'Guam', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('HI', 'Hawaii', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('IA', 'Iowa', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('ID', 'Idaho', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('IL', 'Illinois', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('IN', 'Indiana', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('KS', 'Kansas', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('KY', 'Kentucky', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('LA', 'Louisiana', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('MA', 'Massachusetts', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('MD', 'Maryland', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('ME', 'Maine', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('MI', 'Michigan', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('MN', 'Minnesota', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('MO', 'Missouri', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('MS', 'Mississippi', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('MT', 'Montana', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('NC', 'North Carolina', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('ND', 'North Dakota', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('NE', 'Nebraska', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('NH', 'New Hampshire', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('NJ', 'New Jersey', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('NM', 'New Mexico', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('NV', 'Nevada', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('NY', 'New York', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('OH', 'Ohio', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('OK', 'Oklahoma', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('OR', 'Ore;n', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('PA', 'Pennsylvania', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('PR', 'Puerto Rico', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('RI', 'Rhode Island', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('SC', 'South Carolina', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('SD', 'South Dakota', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('TN', 'Tennessee', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('TX', 'Texas', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('UT', 'Utah', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('VA', 'Virginia', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('VT', 'Vermont', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('WA', 'Washington', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('WI', 'Wisconsin', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('WV', 'West Virginia', 'US');
GO
INSERT INTO State(StateCode, Name, CountryCode)
VALUES('WY', 'Wyoming', 'US');
GO



