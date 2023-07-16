using AppMinimalApi.Models;
using AppMinimalApi.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        app.MapPost("/api/signin", SignIn)
                .WithName("SignIn")
                .Accepts<SignInRequestDTO>("application/json")
                .Produces<APIResponse>(200)
                .Produces(400)
                .AllowAnonymous();

        app.MapPost("/api/signup", SignUp)
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

        return Results.Created($"/api/users/{newUser.Id}", newUser.Id);
    }

    private async static Task<IResult> SignIn(UserManager<IdentityUser> userManager,
          [FromBody] SignUpRequestDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null) 
            return Results.BadRequest("User/Password Invalid!");

        if (!await userManager.CheckPasswordAsync(user, model.Password))
            return Results.BadRequest("User/Password Invalid!");

        var key = Encoding.ASCII.GetBytes("dsjak@1234oldjsalLJKJLDSA");
        var tokenDescriptior = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, model.Email)
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "APP",
            Issuer = "Issuer"
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptior);

        return Results.Ok(new APIResponse { Result = tokenHandler.WriteToken(token), StatusCode = HttpStatusCode.OK});
    }
}
