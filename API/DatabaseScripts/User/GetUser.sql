DECLARE @username NVARCHAR(50)

SET @username = {0}

SELECT [userId]
      ,[username]
      ,[email]
	  ,[password]
FROM [dbo].[Users] WITH(NOLOCK)
WHERE Username = @username