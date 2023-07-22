using AppMinimalApi.DTO;
using FluentValidation;

namespace AppMinimalApi.Validation;

public class ProductDTOValidation : AbstractValidator<ProductDTO>
{
    public ProductDTOValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
    }
}
