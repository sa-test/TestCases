CREATE PROCEDURE [dbo].[sp_CreateUser]
    @name nchar(10),
    @descr text,
    @id int out
AS
    INSERT INTO Users (Name, Description)
    VALUES (@name, @descr)
  
    SET @id=SCOPE_IDENTITY()
GO