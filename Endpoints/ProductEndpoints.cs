using AppMinimalApi.DTO;
using AppMinimalApi.Filters;
using AppMinimalApi.Services.interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AppMinimalApi.Endpoints;

public static class ProductEndpoints
{
    public static void ConfigureProductEndpoints(this WebApplication app)
    {
        app.MapPost("/products", CreateNew)
             .WithName("Create New Product")
             .Accepts<ProductDTO>("application/json")
             .Produces<APIResponse>(200)
             .AddEndpointFilter<BasicValidator<ProductDTO>>()
             .Produces(400);

        app.MapPut("/products", Update)
            .WithName("Update New Product")
            .Accepts<ProductDTO>("application/json")
            .Produces<APIResponse>(200)
            .Produces(400);

        app.MapDelete("/products/{id}", Delete)
            .WithName("Delete Product")
            .Accepts<ProductDTO>("application/json")
            .Produces<APIResponse>(200)
            .Produces(400);

        app.MapGet("/products/{id}", Get)
            .WithName("Get one Product")
            .Produces<APIResponse>(200)
            .Produces(400);

        app.MapGet("/products", GetAll)
           .WithName("Create all Product")
           .Produces<APIResponse>(200)
           .Produces(400);
    }

    private async static Task<IResult> CreateNew(IProductService productService,IValidator<ProductDTO> validator, [FromBody] ProductDTO dto)
    {
        await productService.CreateAsync(dto);
        return Results.Ok();
    }

    private async static Task<IResult> Update(IProductService productService, [FromBody] ProductDTO dto)
    {
        await productService.UpdateAsync(dto);
        return Results.NoContent();
    }

    private async static Task<IResult> Delete(IProductService productService, int Id)
    {
        await productService.RemoveAsync(Id);
        return Results.NoContent();
    }

    private async static Task<IResult> Get(IProductService productService, int Id)
    {
        var productDto = await productService.GetAsync(Id);
        return Results.Ok(new APIResponse() { Result = productDto });
    }

    private async static Task<IResult> GetAll(IProductService productService)
    {
        var productsDto = await productService.GetAllAsync();
        return Results.Ok(new APIResponse { Result = productsDto });
    }
}
