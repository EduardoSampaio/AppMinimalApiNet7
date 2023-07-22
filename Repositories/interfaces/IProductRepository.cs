using AppMinimalApi.Models;
using static Azure.Core.HttpHeader;

namespace AppMinimalApi.Repositories.interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetAsync(int id);
    Task CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task RemoveAsync(int id);
    Task SaveAsync();
}
