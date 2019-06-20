DECLARE @teamId int

SET @teamId = {0}

SELECT *
FROM [dbo].[Teams] WITH(NOLOCK)
WHERE teamId = @teamId