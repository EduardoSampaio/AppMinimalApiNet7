using AppMinimalApi.DTO;
using AppMinimalApi.Models;
using AutoMapper;

namespace AppMinimalApi;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
    }
}
