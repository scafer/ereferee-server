DECLARE @timeStart DATETIME
DECLARE @home_Color NVARCHAR(250)
DECLARE @visitor_Color NVARCHAR(250)
DECLARE @status int
DECLARE @matchOwnerId int
DECLARE @homeTeamId int
DECLARE @visitorId int

SET @timeStart = {0}
SET @home_Color = {1}
SET @visitor_Color = {2}
SET @status = {3}
SET @matchOwnerId = {4}
SET @homeTeamId = {5}
SET @visitorId = {6}


INSERT INTO [dbo].[matches]
           ([timeStart]
           ,[home_color]
           ,[visitor_color]
           ,[status]
           ,[dt_creation]
           ,[matchOwnerId]
           ,[homeTeamId]
           ,[visitorId])
     VALUES
           (@timeStart
           ,@home_Color
           ,@visitor_color
           ,@status
           ,GetDate()
           ,@matchOwnerId
           ,@homeTeamId
           ,@visitorId)