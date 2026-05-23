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

    public async Task<IEnumerable<Product>> GetAllAsync(bool? statusProduct)
    {
        var query = "sp_GetAllProducts";
        var parameters = new { IsActive = statusProduct };

        using (var connection = GetConnection())
        {
            return await connection.QueryAsync<Product>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
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
