﻿DECLARE @matchId int
DECLARE @memberId int
DECLARE @reg int

SET @matchId  = {0}
SET @memberId  = {1}
SET @reg  = {2}

INSERT INTO [dbo].[match_member]
           ([matchId]
           ,[memberId]
           ,[reg])
     VALUES
           ( @matchId
           , @memberId
		   , @reg )