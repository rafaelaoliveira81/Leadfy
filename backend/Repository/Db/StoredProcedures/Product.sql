CREATE PROCEDURE sp_GetProductsPaginado
    @TenantId UNIQUEIDENTIFIER,
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
        WHERE TenantId = @TenantId
            AND (@Status IS NULL OR IsActive = @Status);

    SELECT
        Id,
        Name,
        Description,
        Price,
        IsActive,
        CreatedAt
    FROM Products
        WHERE TenantId = @TenantId
            AND (@Status IS NULL OR IsActive = @Status)
    ORDER BY Id DESC
    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
END;
GO