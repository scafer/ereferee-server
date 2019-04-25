DECLARE @name NVARCHAR(250)

SET @name = {0}

INSERT INTO [dbo].[members]
           ([name])
     VALUES
           (@name)