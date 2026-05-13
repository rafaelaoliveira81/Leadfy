using Domain.Entities;

namespace Application;

public class ProductApp : IProductApp
{
    private readonly IProductRepo _productRepo;
    public ProductApp(IProductRepo productRepo)
    {
        _productRepo = productRepo;
    }
    public async Task<int> AddAsync(Product product)
    {
        ValidateProductInformation(product);

        return await _productRepo.AddAsync(product);
    }
    public async Task<Product> GetByIdAsync(int idProduct)
    {
        return await ValidateProductExistsByIdAsync(idProduct);
    }
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
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepo.GetAllAsync();
    }
    public async Task<IEnumerable<Product>> GetAllByStatusAsync(bool statusProduct)
    {
        return await _productRepo.GetAllByStatusAsync(statusProduct);
    }
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
    public async Task DeleteAsync(int idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        await _productRepo.DeleteAsync(productEntity);
    }
    public async Task DeactivateAsync(int idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        productEntity.Deactivate();

        await _productRepo.UpdateAsync(productEntity);
    }
    public async Task ActivateAsync(int idProduct)
    {
        var productEntity = await ValidateProductExistsByIdAsync(idProduct);

        productEntity.Activate();

        await _productRepo.UpdateAsync(productEntity);
    }

    #region Métodos auxiliares
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
    private async Task<Product> ValidateProductExistsByIdAsync(int idProduct)
    {
        var productEntity = await _productRepo.GetByIdAsync(idProduct);

        if (productEntity == null)
            throw new KeyNotFoundException("Produto não localizado.");

        return productEntity;
    }

    #endregion
}
