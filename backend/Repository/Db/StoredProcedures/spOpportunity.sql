CREATE PROCEDURE sp_CreateOpportunity
    @LeadId INT,
    @ProductId INT,
    @UserId INT,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME,
    @SortOrder INT
AS
BEGIN
    INSERT INTO Opportunities (
        LeadId,
        UserId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        SortOrder
    )
    VALUES (
        @LeadId,
        @UserId,
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
        UserId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        SortOrder
    FROM Opportunities
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetAllOpportunities
AS
BEGIN
    SELECT 
        ID,
        LeadId,
        UserId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        SortOrder
    FROM Opportunities
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