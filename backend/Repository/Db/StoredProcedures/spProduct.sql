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

CREATE PROCEDURE sp_GetProductsPaginado
    @Status INT = NULL,
    @Pagina INT = 1,
    @QuantidadePorPagina INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @Pagina < 1
        SET @Pagina = 1;

    SELECT COUNT(*) AS TotalRegistros
    FROM Products
    WHERE (@Status IS NULL OR IsActive = @Status);

    SELECT
        Id,
        Name,
        Description,
        Price,
        IsActive,
        CreatedAt
    FROM Products
    WHERE (@Status IS NULL OR IsActive = @Status)
    ORDER BY Id DESC
    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
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