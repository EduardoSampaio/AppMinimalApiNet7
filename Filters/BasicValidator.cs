using AppMinimalApi.DTO;
using FluentValidation;
using System.Net;

namespace AppMinimalApi.Filters;

public class BasicValidator<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;
    public BasicValidator(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
        var contextObj = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(T));

        if (contextObj == null)
        {
            return Results.BadRequest(response);
        }
        var result = await _validator.ValidateAsync((T)contextObj);
        if (!result.IsValid)
        {
            response.ErrorMessages.Add(result.Errors.FirstOrDefault().ToString());
            return Results.BadRequest(response);
        }
        return await next(context);
    }
}
