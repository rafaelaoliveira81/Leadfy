using Domain.Entities;
using Application.DTO;

namespace Application;

public class ProductApp : IProductApp
{
    private readonly IProductRepo _productRepo;
    public ProductApp(IProductRepo productRepo)
    {
        _productRepo = productRepo;
    }
    public async Task<string> AddAsync(ProductRequest request)
    {
        ValidateProductInformation(request);

        var product = MapToProductRequest(request);

        return (await _productRepo.CreateAsync(product)).ToString();
    }

    public async Task<ProductResponse> GetByIdAsync(string idProduct)
    {
        var product = await ValidateProductExistsByIdAsync(idProduct);

        return MapToProductResponse(product);
    }
    public async Task<ProductPagedResponse> GetAllAsync(bool? statusProduct, int pagina, int quantidadePorPagina)
    {
        var products = await _productRepo.GetPagedAsync(statusProduct, pagina, quantidadePorPagina);

        var response = products.Dados.Select(p => MapToProductResponse(p)).ToList();

        return new ProductPagedResponse
        {
            TotalRegistros = products.TotalRegistros,
            Dados = response
        };
    }
    public async Task UpdateAsync(ProductRequest request)
    {
        var productEntity = await ValidateProductExistsByIdAsync(request.Id);

        ValidateProductInformation(request);

        productEntity.Name = request.Name;
        productEntity.Description = request.Description;
        productEntity.Price = request.Price;

        await _productRepo.UpdateAsync(productEntity);
    }
    public async Task DeleteAsync(string idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        await _productRepo.DeleteAsync(productEntity);
    }
    public async Task DeactivateAsync(string idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        productEntity.Deactivate();

        await _productRepo.UpdateAsync(productEntity);
    }
    public async Task ActivateAsync(string idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        productEntity.Activate();

        await _productRepo.UpdateAsync(productEntity);
    }

    #region Utils
    private void ValidateProductInformation(ProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Nome do produto é obrigatório.");

        if (request.Name.Length > 150)
            throw new ArgumentException("Nome do produto não pode exceder 150 caracteres.");

        if (request.Description != null && request.Description.Length > 1000)
            throw new ArgumentException("Descrição do produto não pode exceder 1000 caracteres.");

        if (request.Price <= 0)
            throw new ArgumentException("Preço do produto deve ser maior que zero.");
    }
    private async Task<Product> ValidateProductExistsByIdAsync(string idProduct)
    {
        if (!Guid.TryParse(idProduct, out var productGuid))
            throw new ArgumentException("O identificador do produto é inválido.");

        var productEntity = await _productRepo.GetByIdAsync(productGuid);

        if (productEntity == null)
            throw new KeyNotFoundException("Produto não localizado.");

        return productEntity;
    }

    private ProductResponse MapToProductResponse(Product product)
    {
        var productResponse = new ProductResponse
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            IsActive = product.IsActive
        };

        return productResponse;
    }

    private static Product MapToProductRequest(ProductRequest request)
    {
        return new Product
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Price = request.Price,
            IsActive = true
        };
    }


    #endregion
}
