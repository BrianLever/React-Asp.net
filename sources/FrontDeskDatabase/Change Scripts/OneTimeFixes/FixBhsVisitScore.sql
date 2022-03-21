begin tran

UPDATE v 
SET  
AlcoholUseFlagScoreLevel = sr_a.Score, 
SubstanceAbuseFlagScoreLevel = sr_s.Score, 
DepressionFlagScoreLevel = sr_d.Score, 
PartnerViolenceFlagScoreLevel = sr_h.Score 
from dbo.BhsVisit v 
LEFT JOIN dbo.ScreeningSectionResult sr_a ON v.ScreeningResultID = sr_a.ScreeningResultID and sr_a.ScreeningSectionID = 'CAGE' 
LEFT JOIN dbo.ScreeningSectionResult sr_s ON v.ScreeningResultID = sr_s.ScreeningResultID and sr_s.ScreeningSectionID = 'DAST' 
LEFT JOIN dbo.ScreeningSectionResult sr_d ON v.ScreeningResultID = sr_d.ScreeningResultID and sr_d.ScreeningSectionID = 'PHQ-9'
LEFT JOIN dbo.ScreeningSectionResult sr_h ON v.ScreeningResultID = sr_h.ScreeningResultID and sr_h.ScreeningSectionID = 'HITS'


select v.Id, v.ScreeningResultID, AlcoholUseFlagScoreLevel, sr_a.Score, SubstanceAbuseFlagScoreLevel, sr_s.Score, DepressionFlagScoreLevel, sr_d.Score, PartnerViolenceFlagScoreLevel, sr_h.Score 
from dbo.BhsVisit v 
LEFT JOIN dbo.ScreeningSectionResult sr_a ON v.ScreeningResultID = sr_a.ScreeningResultID and sr_a.ScreeningSectionID = 'CAGE' 
LEFT JOIN dbo.ScreeningSectionResult sr_s ON v.ScreeningResultID = sr_s.ScreeningResultID and sr_s.ScreeningSectionID = 'DAST' 
LEFT JOIN dbo.ScreeningSectionResult sr_d ON v.ScreeningResultID = sr_d.ScreeningResultID and sr_d.ScreeningSectionID = 'PHQ-9'
LEFT JOIN dbo.ScreeningSectionResult sr_h ON v.ScreeningResultID = sr_h.ScreeningResultID and sr_h.ScreeningSectionID = 'HITS'
 
 commit tran