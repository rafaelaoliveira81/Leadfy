CREATE PROCEDURE sp_CreateOpportunity
    @LeadId INT,
    @ProductId INT,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME,
    @SortOrder INT
AS
BEGIN
    INSERT INTO Opportunities (
        LeadId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        IsActive,
        SortOrder
    )
    VALUES (
        @LeadId,
        @ProductId,
        @Stage,
        @Amount,
        @ExpectedCloseDate,
        GETUTCDATE(),
        1,
        @SortOrder
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
END;
GO

CREATE PROCEDURE sp_GetOpportunityById
    @ID INT
AS
BEGIN
    SELECT 
        ID,
        LeadId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        IsActive,
        SortOrder
    FROM Opportunities
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetAllOpportunities
    @IsActive BIT = NULL
AS
BEGIN
    SELECT 
        ID,
        LeadId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        IsActive,
        SortOrder
    FROM Opportunities
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY CreatedAt DESC;
END;
GO

CREATE PROCEDURE sp_UpdateOpportunity
    @ID INT,
    @LeadId INT,
    @ProductId INT,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME,
    @IsActive BIT,
    @SortOrder INT
AS
BEGIN
    UPDATE Opportunities
    SET 
        LeadId = @LeadId,
        ProductId = @ProductId,
        Stage = @Stage,
        Amount = @Amount,
        ExpectedCloseDate = @ExpectedCloseDate,
        IsActive = @IsActive,
        SortOrder = @SortOrder
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteOpportunity
    @ID INT
AS
BEGIN
    DELETE FROM Opportunities
    WHERE ID = @ID;
END;
GO