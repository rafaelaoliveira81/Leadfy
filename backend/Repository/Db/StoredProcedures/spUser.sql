CREATE PROCEDURE sp_CreateUser
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(500)
AS
BEGIN
    INSERT INTO Users (
        Name,
        Email,
        PasswordHash,
        IsActive,
        CreatedAt
    )
    VALUES (
        @Name,
        @Email,
        @PasswordHash,
        1,
        GETUTCDATE()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
END;
GO

CREATE PROCEDURE sp_GetUserById
    @ID INT
AS
BEGIN
    SELECT
        ID,
        Name,
        Email,
        PasswordHash,
        IsActive,
        CreatedAt
    FROM Users
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetUserByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT
        ID,
        Name,
        Email,
        PasswordHash,
        IsActive,
        CreatedAt
    FROM Users
    WHERE Email = @Email;
END;
GO

CREATE PROCEDURE sp_GetAllUsers
    @IsActive BIT = NULL
AS
BEGIN
    SELECT
        ID,
        Name,
        Email,
        PasswordHash,
        IsActive,
        CreatedAt
    FROM Users
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY CreatedAt DESC;
END;
GO

CREATE PROCEDURE sp_UpdateUser
    @ID INT,
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(500),
    @IsActive BIT
AS
BEGIN
    UPDATE Users
    SET
        Name = @Name,
        Email = @Email,
        PasswordHash = @PasswordHash,
        IsActive = @IsActive
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteUser
    @ID INT
AS
BEGIN
    DELETE FROM Users
    WHERE ID = @ID;
END;
GO