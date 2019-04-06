DECLARE @username NVARCHAR(50)

SET @username = {0}

SELECT CASE
    WHEN EXISTS (SELECT TOP 1 * FROM [dbo].[Users]
                    WHERE Username = @username) THEN 1
        ELSE 0
    END 