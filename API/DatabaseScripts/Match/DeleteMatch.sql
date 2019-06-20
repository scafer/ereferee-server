DECLARE @matchId int

SET @matchId = {0}

UPDATE 
    [dbo].[matches]
SET 
    [status] = -1
WHERE  
    matchId = @matchId