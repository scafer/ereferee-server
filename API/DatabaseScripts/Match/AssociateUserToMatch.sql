DECLARE @userId int
DECLARE @matchId int
DECLARE @role int

SET @userId  = {0}
SET @matchId  = {1}
SET @role  = {2}

INSERT INTO [dbo].[user_match]
           ([userId]
           ,[matchId]
           ,[role])
     VALUES
           ( @userId
           , @matchId
		   , @role )