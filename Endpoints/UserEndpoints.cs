using AppMinimalApi.DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace AppMinimalApi.Endpoints;

public static class UserEndpoints
{
    public static void ConfigureAuthEndpoints(this WebApplication app)
    {

        app.MapPost("/auth/signin", SignIn)
                .WithName("SignIn")
                .Accepts<SignInRequestDTO>("application/json")
                .Produces<APIResponse>(200)
                .Produces(400)
                .AllowAnonymous();

        app.MapPost("/auth/signup", SignUp)
                .WithName("SignUp")
                .Accepts<SignUpRequestDTO>("application/json")
                .Produces<APIResponse>(200).Produces(400)
                .AllowAnonymous();
    }

    private async static Task<IResult> SignUp(UserManager<IdentityUser> userManager,
            [FromBody] SignUpRequestDTO model)
    {
        var newUser = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await userManager.CreateAsync(newUser, model.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors.First());
        }

        return Results.Created($"/auth/user/{newUser.Id}", newUser.Id);
    }

    private async static Task<IResult> SignIn(UserManager<IdentityUser> userManager,
          [FromBody] SignUpRequestDTO model, IConfiguration configuration)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null) 
            return Results.BadRequest("User/Password Invalid!");

        if (!await userManager.CheckPasswordAsync(user, model.Password))
            return Results.BadRequest("User/Password Invalid!");

        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]!);
        var tokenDescriptior = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, model.Email)
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddHours(5)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptior);

        return Results.Ok(new APIResponse { Result = new { token = tokenHandler.WriteToken(token) }, StatusCode = HttpStatusCode.OK});
    }
}
