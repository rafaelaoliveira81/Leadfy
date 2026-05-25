CREATE PROCEDURE sp_CreateLead
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @PhoneNumber NVARCHAR(50)
AS
BEGIN
    INSERT INTO Leads (Name, Email, PhoneNumber, IsActive, CreatedAt)
    VALUES (@Name, @Email, @PhoneNumber, 1, GETUTCDATE());

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END;
GO

CREATE PROCEDURE sp_GetLeadById
    @Id INT
AS
BEGIN
    SELECT 
        Id,
        Name,
        Email,
        PhoneNumber,
        IsActive,
        CreatedAt
    FROM Leads
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_GetLeadsPaginado
    @Pagina INT = 1,
    @QuantidadePorPagina INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @Pagina < 1
        SET @Pagina = 1;

    SELECT
        Id,
        Name,
        Email,
        PhoneNumber,
        IsActive,
        CreatedAt
    FROM Leads
    ORDER BY ID DESC
    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
END;
GO

CREATE PROCEDURE sp_UpdateLead
    @Id INT,
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @PhoneNumber NVARCHAR(50),
    @IsActive BIT
AS
BEGIN
    UPDATE Leads
    SET 
        Name = @Name,
        Email = @Email,
        PhoneNumber = @PhoneNumber,
        IsActive = @IsActive
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_DeleteLead
    @Id INT
AS
BEGIN
    DELETE FROM Leads
    WHERE Id = @Id;
END;
GO