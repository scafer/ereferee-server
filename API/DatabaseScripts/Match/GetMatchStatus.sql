DECLARE @matchId int

SET @matchId  = {0}

SELECT [Status] FROM matches m WITH(NOLOCK)
WHERE m.matchId = @matchId