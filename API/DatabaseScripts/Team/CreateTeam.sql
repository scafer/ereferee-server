﻿DECLARE @name NVARCHAR(250)
DECLARE @color NVARCHAR(512)

SET @name = {0}
SET @color = {1}


INSERT INTO [dbo].[teams]
           ([name]
           ,[color])
     VALUES
           (@name
           ,@color)