using Microsoft.EntityFrameworkCore;
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
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product.Id;
    }
    public async Task<Product> GetByIdAsync(int idProduct)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == idProduct);
    }
    public async Task<IEnumerable<Product>> GetByNameContainingAsync(string nameProduct)
    {
        return await _context.Products
            .Where(p => EF.Functions.Like(p.Name, $"%{nameProduct}%"))
            .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetAllByStatusAsync(bool statusProduct)
    {
        return await _context.Products
            .Where(p => p.IsActive == statusProduct)
            .ToListAsync();
    }
    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
