using Domain.Entities;

namespace Application;

/// <summary>
/// Serviço de aplicação responsável por orquestrar os casos de uso relacionados a products.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Application.
/// Sua responsabilidade é validar entradas, aplicar regras de fluxo,
/// coordenar chamadas ao domínio e persistir alterações por meio do repositório.
/// </remarks>
public class ProductApp : IProductApp
{
    /// <summary>
    /// Repositório responsável pelo acesso e persistência dos products.
    /// </summary>
    private readonly IProductRepo _productRepo;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ProductApp"/>.
    /// </summary>
    /// <param name="productRepo">Repositório de products.</param>
    public ProductApp(IProductRepo productRepo)
    {
        _productRepo = productRepo;
    }

    /// <summary>
    /// Adiciona um novo product ao sistema.
    /// </summary>
    /// <param name="product">Entidade de product a ser cadastrada.</param>
    /// <returns>Retorna o identificador do product criado.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do product são inválidos.
    /// </exception>
    public async Task<int> AddAsync(Product product)
    {
        ValidateProductInformation(product);

        return await _productRepo.AddAsync(product);
    }

    /// <summary>
    /// Obtém um product pelo seu identificador.
    /// </summary>
    /// <param name="idProduct">ID do product.</param>
    /// <returns>Product encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o product não é localizado.
    /// </exception>
    public async Task<Product> GetByIdAsync(int idProduct)
    {
        return await ValidateProductExistsByIdAsync(idProduct);
    }

    /// <summary>
    /// Busca products cujo nome contenha o valor informado.
    /// </summary>
    /// <param name="nameProduct">
    /// Texto utilizado para filtrar os products pelo nome.
    /// Não pode ser nulo, vazio ou composto apenas por espaços.
    /// </param>
    /// <returns>
    /// Uma coleção de products que possuem o nome contendo o valor informado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o parâmetro <paramref name="nameProduct"/> é nulo ou inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando nenhum product é encontrado para o critério informado.
    /// </exception>
    public async Task<IEnumerable<Product>> GetByNameContainingAsync(string nameProduct)
    {
        if (string.IsNullOrWhiteSpace(nameProduct))
            throw new ArgumentException("Nome do product não pode ser vazio.");

        nameProduct = nameProduct.Trim();

        var productEntity = await _productRepo.GetByNameContainingAsync(nameProduct);

        if (productEntity == null || !productEntity.Any())
            throw new KeyNotFoundException("Produto não localizado.");

        return productEntity;
    }

    /// <summary>
    /// Obtém todos os products cadastrados.
    /// </summary>
    /// <returns>Coleção com todos os products.</returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepo.GetAllAsync();
    }

    /// <summary>
    /// Obtém todos os products filtrando pelo status.
    /// </summary>
    /// <param name="statusProduct">
    /// Status desejado para o filtro (true = ativo, false = inativo).
    /// </param>
    /// <returns>Coleção de products com o status informado.</returns>
    public async Task<IEnumerable<Product>> GetAllByStatusAsync(bool statusProduct)
    {
        return await _productRepo.GetAllByStatusAsync(statusProduct);
    }

    /// <summary>
    /// Atualiza os dados de um product existente.
    /// </summary>
    /// <param name="product">Product com os dados atualizados.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do product são inválidos.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o product a ser atualizado não é localizado.
    /// </exception>
    public async Task UpdateAsync(Product product)
    {
        var productEntity = await ValidateProductExistsByIdAsync(product.Id);

        ValidateProductInformation(product);

        productEntity.Name = product.Name;
        productEntity.Description = product.Description;
        productEntity.Price = product.Price;
        productEntity.IsActive = product.IsActive;

        await _productRepo.UpdateAsync(productEntity);
    }

    /// <summary>
    /// Remove um product do sistema.
    /// </summary>
    /// <param name="idProduct">ID do product a ser removido.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o product não é localizado.
    /// </exception>
    /// <remarks>
    /// Este método realiza remoção física do registro.
    /// </remarks>
    public async Task DeleteAsync(int idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        await _productRepo.DeleteAsync(productEntity);
    }

    /// <summary>
    /// Desativa um product no sistema.
    /// </summary>
    /// <param name="idProduct">ID do product a ser desativado.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o product não é localizado.
    /// </exception>
    public async Task DeactivateAsync(int idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        productEntity.Deactivate();

        await _productRepo.UpdateAsync(productEntity);
    }

    /// <summary>
    /// Ativa um product no sistema.
    /// </summary>
    /// <param name="idProduct">ID do product a ser ativado.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o product não é localizado.
    /// </exception>
    public async Task ActivateAsync(int idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        productEntity.Activate();

        await _productRepo.UpdateAsync(productEntity);
    }

    #region Métodos auxiliares

    /// <summary>
    /// Valida as regras básicas e de negócio do product.
    /// </summary>
    /// <param name="product">Product a ser validado.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando dados obrigatórios são inválidos.
    /// </exception>
    private void ValidateProductInformation(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Nome do produto é obrigatório.");

        if (product.Name.Length > 150)
            throw new ArgumentException("Nome do produto não pode exceder 150 caracteres.");

        if (product.Description != null && product.Description.Length > 1000)
            throw new ArgumentException("Descrição do produto não pode exceder 1000 caracteres.");

        if (product.Price <= 0)
            throw new ArgumentException("Preço do produto deve ser maior que zero.");
    }

    /// <summary>
    /// Valida se um product existe pelo id.
    /// </summary>
    /// <param name="idProduct">ID do product.</param>
    /// <returns>Product encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o product não é localizado.
    /// </exception>
    private async Task<Product> ValidateProductExistsByIdAsync(int idProduct)
    {
        var productEntity = await _productRepo.GetByIdAsync(idProduct);

        if (productEntity == null)
            throw new KeyNotFoundException("Produto não localizado.");

        return productEntity;
    }

    #endregion
}
