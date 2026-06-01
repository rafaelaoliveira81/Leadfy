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

CREATE PROCEDURE sp_GetUsersPaginado
    @IsActive BIT = NULL,
    @Pagina INT = 1,
    @QuantidadePorPagina INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @Pagina < 1
        SET @Pagina = 1;
    
    SELECT COUNT(*) AS TotalRegistros
    FROM Users
    WHERE (@IsActive IS NULL OR IsActive = @IsActive);

    SELECT
        ID,
        Name,
        Email,
        PasswordHash,
        IsActive,
        CreatedAt
    FROM Users
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY Name DESC
    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
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
    SET NOCOUNT ON;

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