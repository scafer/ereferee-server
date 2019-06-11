DECLARE @email NVARCHAR(100)

SET @email = {0}

SELECT CASE
    WHEN EXISTS (SELECT TOP 1 * FROM [dbo].[Users] WITH(NOLOCK)
                    WHERE Email = @email) THEN 1
        ELSE 0
    END 