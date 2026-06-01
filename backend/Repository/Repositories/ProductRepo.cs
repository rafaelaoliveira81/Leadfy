using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class ProductRepo : BaseRepo, IProductRepo
{
    public ProductRepo(CRMContext context) : base(context)
    {
    }
    public async Task<int> AddAsync(Product product)
    {
        var query = "sp_CreateProduct";
        var parameters = new
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };

        using (var connection = GetConnection())
        {
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task<Product> GetByIdAsync(int idProduct)
    {
        var query = "sp_GetProductById";
        var parameters = new { ID = idProduct };

        using (var connection = GetConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Product>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

   public async Task<PagedResult<Product>> GetPagedAsync(
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(
            "sp_GetProductsPaginado",
            new
            {
                Status = isActive.HasValue ? (isActive.Value ? 1 : 0) : (int?)null,
                Pagina = pagina,
                QuantidadePorPagina = quantidadePorPagina
            },
            commandType: System.Data.CommandType.StoredProcedure
        );

        var totalRegistros = await multi.ReadFirstAsync<int>();
        var products = (await multi.ReadAsync<Product>()).ToList();

        return new PagedResult<Product>
        {
            TotalRegistros = totalRegistros,
            Dados = products
        };
    }
    
    public async Task UpdateAsync(Product product)
    {
        var query = "sp_UpdateProduct";
        var parameters = new
        {
            ID = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            IsActive = product.IsActive
        };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
    public async Task DeleteAsync(Product product)
    {
        var query = "sp_DeleteProduct";
        var parameters = new { ID = product.Id };

        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
