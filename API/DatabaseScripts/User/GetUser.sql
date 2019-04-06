DECLARE @username NVARCHAR(50)

SET @username = {0}

SELECT * FROM [dbo].[Users]
WHERE Username = @username