DECLARE @matchId int

SET @matchId = {0}

SELECT [matchId] AS MatchID
      ,[userId] AS UserID
      ,[memberId] AS MemberID
      ,[eventId] AS EventType
      ,[dt] AS DateTime
      ,[eventDescription] AS Event_Description
FROM [dbo].[match_events]
WHERE matchId = @matchId 