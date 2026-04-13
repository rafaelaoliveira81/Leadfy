using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

/// <summary>
/// Repositório responsável pela persistência e consulta de products.
/// Implementa operações CRUD utilizando Entity Framework Core.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Repository e deve conter apenas
/// lógica de acesso a dados, sem regras de negócio.
/// </remarks>
public class ProductRepo : BaseRepo, IProductRepo
{
    public ProductRepo(CRMContext context) : base(context)
    {
    }

    /// <summary>
    /// Adiciona um novo product no banco de dados.
    /// </summary>
    /// <param name="product">Entidade do product a ser persistida.</param>
    /// <returns>Retorna o ID do product gerado após a inserção.</returns>
    public async Task<int> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product.Id;
    }

    /// <summary>
    /// Busca um product pelo seu identificador único.
    /// </summary>
    /// <param name="idProduct">ID do product.</param>
    /// <returns>Product encontrado ou null caso não exista.</returns>
    public async Task<Product> GetByIdAsync(int idProduct)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == idProduct);
    }

    /// <summary>
    /// Busca products cujo nome contenha o valor informado.
    /// </summary>
    /// <param name="nameProduct">
    /// Texto utilizado para filtrar os products pelo nome.
    /// A busca é case-insensitive.
    /// </param>
    /// <returns>
    /// Uma coleção de products que possuem o nome contendo o valor informado.
    /// Retorna uma lista vazia caso nenhum product seja encontrado.
    /// </returns>
    public async Task<IEnumerable<Product>> GetByNameContainingAsync(string nameProduct)
    {
        return await _context.Products
            .Where(p => EF.Functions.Like(p.Name, $"%{nameProduct}%"))
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todos os products cadastrados.
    /// </summary>
    /// <returns>Lista de products.</returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    /// <summary>
    /// Retorna todos os products filtrando pelo status (ativo/inativo).
    /// </summary>
    /// <param name="statusProduct">Status do product (true = ativo, false = inativo).</param>
    /// <returns>Lista de products filtrados.</returns>
    public async Task<IEnumerable<Product>> GetAllByStatusAsync(bool statusProduct)
    {
        return await _context.Products
            .Where(p => p.IsActive == statusProduct)
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza os dados de um product existente.
    /// </summary>
    /// <param name="product">Product com dados atualizados.</param>
    /// <remarks>
    /// O Entity Framework irá rastrear as alterações e persistir no banco.
    /// </remarks>
    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove um product do banco de dados.
    /// </summary>
    /// <param name="product">Product a ser removido.</param>
    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
