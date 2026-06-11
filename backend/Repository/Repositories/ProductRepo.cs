using Microsoft.EntityFrameworkCore;
using Dapper;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class ProductRepo : BaseRepository<Product>, IProductRepo
{
    public ProductRepo(CRMContext context) : base(context)
    {
    }

   public async Task<PagedResult<Product>> GetPagedAsync(
        Guid tenantId,
        bool? isActive,
        int pagina,
        int quantidadePorPagina)
    {
        using var connection = GetConnection();

        using var multi = await connection.QueryMultipleAsync(
            "sp_GetProductsPaginado",
            new
            {
                TenantId = tenantId,
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
}
