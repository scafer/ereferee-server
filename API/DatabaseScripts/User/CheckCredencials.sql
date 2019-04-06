DECLARE @username   NVARCHAR(50)
DECLARE @email      NVARCHAR(100)

SET @username       = {0}
SET @email          = {1}

SELECT CASE
    WHEN EXISTS (SELECT TOP 1 * FROM [dbo].[Users]
                    WHERE Username = @username
                    AND Email = @email) THEN 1
        ELSE 0
    END 