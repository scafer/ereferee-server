DECLARE @matchId int

SET @matchId = {0}

SELECT [matchId]
      ,[userId]
      ,[memberId]
      ,[eventId]
      ,[dt]
      ,[eventDescription]
FROM [dbo].[match_events]
WHERE matchId = @matchId 