DECLARE @teamId int
DECLARE @memberId int
DECLARE @status int
DECLARE @role int
DECLARE @number int

SET @teamId  = {0}
SET @memberId  = {1}
SET @status  = {2}
SET @role  = {3}
SET @number  = {4}


INSERT INTO [dbo].[team_member]
           ([teamId]
           ,[memberId]
           ,[status]
           ,[role]
           ,[number])
     VALUES 
           ( @teamId
		   , @memberId
		   , @status
		   , @role
		   , @number)