DECLARE @eventId INT
DECLARE @matchId INT
DECLARE @userId INT
DECLARE @memberId INT
DECLARE @eventDescription NVARCHAR(250)

SET @eventId = {0}
SET @matchId = {1}
SET @userId = {2}
SET @memberId = {3}
SET @eventDescription = {4}

INSERT INTO [dbo].[match_events]
           ([matchId]
		    ,[userId]
			,[memberId]
			,[eventId]
			,[dt]
			,[eventDescription])
     VALUES
           (@matchId
		    ,@userId
			,@memberId
			,@eventId
			,GetDate()
			,@eventDescription)