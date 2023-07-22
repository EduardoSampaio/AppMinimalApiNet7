using AppMinimalApi.DTO;
using AppMinimalApi.Exceptions;
using AppMinimalApi.Models;
using AppMinimalApi.Repositories.interfaces;
using AppMinimalApi.Services.interfaces;
using AutoMapper;

namespace AppMinimalApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        this._productRepository = productRepository;
        this._mapper = mapper;
    }

    public Task CreateAsync(ProductDTO productDTO)
    {
        var product = _mapper.Map<Product>(productDTO);
        _productRepository.CreateAsync(product);
        _productRepository.SaveAsync();
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllAsync()
    {
       var products = await _productRepository.GetAllAsync();
       return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetAsync(int id)
    {
        var product = await _productRepository.GetAsync(id);
        return _mapper.Map<ProductDTO>(product);
    }

    public Task RemoveAsync(int id)
    {
       _productRepository.RemoveAsync(id);
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(ProductDTO productDTO)
    {

        var product = await _productRepository.GetAsync(productDTO.Id) ?? throw new ObjectNotFoundException("Product Not found");
        product.Id = productDTO.Id;
        product.Name = productDTO.Name;
        product.Description = productDTO.Description;
        product.Price = productDTO.Price;
        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveAsync();
    }
}
