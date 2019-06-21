DECLARE @userId INT
DECLARE @eventId INT
DECLARE @matchId INT
DECLARE @memberId INT
DECLARE @eventDescription NVARCHAR(250)
DECLARE @matchTime NVARCHAR(50)

SET @userId = {0}
SET @eventId = {1}
SET @matchId = {2}
SET @memberId = {3}
SET @eventDescription = {4}
SET @matchTime = {5}

INSERT INTO [dbo].[match_events]
           ( [matchId]
		    ,[userId]
			,[memberId]
			,[eventId]
			,[dt]
			,[eventDescription]
			,[match_time])
     VALUES
           (  @matchId
		    , @userId
			, @memberId
			, @eventId
			, GetDate()
			, @eventDescription
			, @matchTime)