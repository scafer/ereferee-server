DECLARE @userId int

SET @userId = {0}

SELECT * FROM matches WITH(NOLOCK)
WHERE matchOwnerId = @userId