CREATE PROCEDURE sp_CreateOwner
    @Name NVARCHAR(255),
    @UserID INT
AS
BEGIN
    INSERT INTO Owners (Name, UserID, IsActive, CreatedAt)
    VALUES (@Name, @UserID, 1, GETDATE());

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO

CREATE PROCEDURE sp_GetOwnerById
    @ID INT
AS
BEGIN
    SELECT 
        ID,
        Name,
        UserID,
        IsActive,
        CreatedAt
    FROM Owners
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetAllOwners
    @IsActive BIT = NULL
AS
BEGIN
    SELECT 
        ID,
        Name,
        UserID,
        IsActive,
        CreatedAt
    FROM Owners
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY Name DESC;
END;
GO

CREATE PROCEDURE sp_UpdateOwner
    @ID INT,
    @Name NVARCHAR(255),
    @UserID INT,
    @IsActive BIT
AS
BEGIN
    UPDATE Owners
    SET 
        Name = @Name,
        UserID = @UserID,
        IsActive = @IsActive
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteOwner
    @ID INT
AS
BEGIN
    UPDATE Owners
    SET IsActive = 0
    WHERE ID = @ID;
END;
GO