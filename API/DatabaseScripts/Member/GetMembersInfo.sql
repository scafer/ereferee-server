DECLARE @teamId int

SET @teamId = {0}

SELECT  tm.teamId as TeamId
      , tm.memberId as MemberId
      , tm.[status] as Status
      , tm.[role] as Role
      , tm.number as Number
      , tm.dayStart as DayStart
      , tm.dayEnd as DayEnd
	  , m.[name] as Name
  FROM [dbo].[team_member] tm
  INNER JOIN [dbo].[members] m on tm.memberId = m.memberID
  WHERE teamId = @teamId