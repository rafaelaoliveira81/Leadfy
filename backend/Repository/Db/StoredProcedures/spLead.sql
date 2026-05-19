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

CREATE PROCEDURE sp_GetAllLeads
    @IsActive BIT = NULL
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
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY CreatedAt DESC;
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