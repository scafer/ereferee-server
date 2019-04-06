DECLARE @username NVARCHAR(50)
DECLARE @password NVARCHAR(MAX)
DECLARE @email NVARCHAR(100)

SET @username = {0}
SET @password = {1}
SET @email = {2}

UPDATE 
    [dbo].[Users]
SET 
    [Password] = @password
WHERE  
    Username = @username
    AND Email = @email; 