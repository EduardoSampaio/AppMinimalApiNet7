using AppMinimalApi.DTO;

namespace AppMinimalApi.Services.interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllAsync();
    Task<ProductDTO> GetAsync(int id);
    Task CreateAsync(ProductDTO productDTO);
    Task UpdateAsync(ProductDTO productDTO);
    Task RemoveAsync(int id);
}
