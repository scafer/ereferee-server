DECLARE @matchId int

SET @matchId = {0}

SELECT * FROM matches WITH(NOLOCK)
WHERE matchId = @matchId 
AND [status] = 2