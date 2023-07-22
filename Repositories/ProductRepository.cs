using AppMinimalApi.EF;
using AppMinimalApi.Models;
using AppMinimalApi.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AppMinimalApi.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(Product product)
    {
        await _db.AddAsync(product);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Product> GetAsync(int id)
    {
        return await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task RemoveAsync(int id)
    {
        var product = await GetAsync(id);
        EntityEntry entityEntry = _db.Entry(product);
        entityEntry.State = EntityState.Deleted;
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

    public Task UpdateAsync(Product product)
    {
        EntityEntry entityEntry = _db.Entry(product);
        entityEntry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
