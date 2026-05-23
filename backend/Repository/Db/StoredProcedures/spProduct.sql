CREATE PROCEDURE sp_CreateProduct
    @Name NVARCHAR(150),
    @Description NVARCHAR(1000),
    @Price DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO Products (Name, Description, Price, IsActive, CreatedAt)
    VALUES (@Name, @Description, @Price, 1, GETDATE());

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO

CREATE PROCEDURE sp_GetProductById
    @ID INT
AS
BEGIN
    SELECT 
        ID,
        Name,
        Description,
        Price,
        IsActive,
        CreatedAt
    FROM Products
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetAllProducts
    @IsActive BIT = NULL
AS
BEGIN
    SELECT 
        ID,
        Name,
        IsActive,
        CreatedAt
    FROM Products
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY Name DESC;
END;
GO

CREATE PROCEDURE sp_UpdateProduct
    @ID INT,
    @Name NVARCHAR(150),
    @Description NVARCHAR(1000),
    @Price DECIMAL(18, 2),
    @IsActive BIT
AS
BEGIN
    UPDATE Products
    SET 
        Name = @Name,
        Description = @Description,
        Price = @Price,
        IsActive = @IsActive
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteProduct
    @ID INT
AS
BEGIN
    DELETE FROM Products
    WHERE ID = @ID;
END;
GO