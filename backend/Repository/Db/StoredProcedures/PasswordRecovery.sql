CREATE PROCEDURE sp_CreatePasswordRecovery
    @UserId INT,
    @Email NVARCHAR(255),
    @Token NVARCHAR(500),
    @ExpiresAt DATETIME
AS
BEGIN
    INSERT INTO PasswordRecoveries (
        UserId,
        Email,
        Token,
        CreatedAt,
        ExpiresAt,
        IsActive
    )
    VALUES (
        @UserId,
        @Email,
        @Token,
        GETUTCDATE(),
        @ExpiresAt,
        1
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
END;
GO

CREATE PROCEDURE sp_GetPasswordRecoveryById
    @ID INT
AS
BEGIN
    SELECT
        ID,
        UserId,
        Email,
        Token,
        CreatedAt,
        ExpiresAt,
        IsActive
    FROM PasswordRecoveries
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetAllPasswordRecoveries
    @IsActive BIT = NULL
AS
BEGIN
    SELECT
        ID,
        UserId,
        Email,
        Token,
        CreatedAt,
        ExpiresAt,
        IsActive
    FROM PasswordRecoveries
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY CreatedAt DESC;
END;
GO

CREATE PROCEDURE sp_UpdatePasswordRecovery
    @ID INT,
    @UserId INT,
    @Email NVARCHAR(255),
    @Token NVARCHAR(500),
    @ExpiresAt DATETIME,
    @IsActive BIT
AS
BEGIN
    UPDATE PasswordRecoveries
    SET
        UserId = @UserId,
        Email = @Email,
        Token = @Token,
        ExpiresAt = @ExpiresAt,
        IsActive = @IsActive
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeletePasswordRecovery
    @ID INT
AS
BEGIN
    DELETE FROM PasswordRecoveries
    WHERE ID = @ID;
END;
GO