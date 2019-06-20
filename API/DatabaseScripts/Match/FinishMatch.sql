DECLARE @matchId int
DECLARE @homeScore int
DECLARE @visitorScore int

SET @matchId = {0}
SET @homeScore = {1}
SET @visitorScore = {2}

UPDATE 
    [dbo].[matches]
SET 
	  [status] = 2
	, [home_score] = @homeScore
	, [visitor_score] = @visitorScore
    , timeEnd = GETDATE()
WHERE  
    matchId = @matchId