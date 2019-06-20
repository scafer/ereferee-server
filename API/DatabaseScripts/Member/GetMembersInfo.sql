DECLARE @teamId int

SET @teamId = {0}

SELECT [teamId]
      ,[memberId]
      ,[status]
      ,[role]
      ,[number]
      ,[dayStart]
      ,[dayEnd]
  FROM [dbo].[team_member]
  WHERE teamId = @teamId