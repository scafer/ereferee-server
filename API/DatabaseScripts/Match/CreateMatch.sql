DECLARE @home_Color NVARCHAR(250)
DECLARE @visitor_Color NVARCHAR(250)
DECLARE @status int
DECLARE @matchOwnerId int
DECLARE @homeTeamId int
DECLARE @visitorId int

SET @home_Color = {0}
SET @visitor_Color = {1}
SET @status = {2}
SET @matchOwnerId = {3}
SET @homeTeamId = {4}
SET @visitorId = {5}


INSERT INTO [dbo].[matches]
           ([home_color]
           ,[visitor_color]
           ,[status]
           ,[dt_creation]
           ,[matchOwnerId]
           ,[homeTeamId]
           ,[visitorId])
     VALUES
           (@home_Color
           ,@visitor_color
           ,@status
           ,GetDate()
           ,@matchOwnerId
           ,@homeTeamId
           ,@visitorId)