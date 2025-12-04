using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Trazert_API.Database;
using Trazert_API.Endpoints.Usuarios.DTOs;
using Trazert_API.Services;

namespace Trazert_API.Endpoints.Usuarios;

public class Endpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/usuarios")
            .WithTags("Usuarios");
        
        group.MapGet("/", GetUsuarios);
        
        group.MapPost("/login", PostLogin);

        group.MapPost("/logout", PostLogout)
            .RequireAuthorization();

        // Válida que el token de acceso siga vigente.
        group.MapPost("/logged", PostLogged)
            .RequireAuthorization();
    }

    private static async Task<GetUsuarioResponse[]> GetUsuarios(DatabaseContext context)
    {
        return await context.UsuariosActivos.Select(u => new GetUsuarioResponse(u.Id, u.Nombre)).ToArrayAsync();
    }

    private static async Task<IResult> PostLogin(IOptions<JwtSettings> config, HttpContext http, DatabaseContext context, PasswordHasher hasher, TokenProvider provider, [FromBody] PostLoginRequest request)
    {
        var user = await context.UsuariosActivos.FirstOrDefaultAsync(u => u.Nombre == request.Nombre);

        if (user is null || !hasher.Verify(request.Password, user.Hash))
        {
            return TypedResults.Unauthorized();
        }

        var jwt = provider.Create(user);
        http.Response.Cookies.Append(Constants.Token, jwt, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddHours(config.Value.ExpiryInMinutes)
        });

        return TypedResults.Ok(new GetUsuarioResponse(user.Id, user.Nombre));
    }

    private static IResult PostLogout(HttpContext context, TokenBlackList blackList)
    {
        var token = context.Request.Cookies[Constants.Token];
        
        if (string.IsNullOrEmpty(token))
            return TypedResults.Unauthorized();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        blackList.Add(token, jwtToken.ValidTo);
        
        context.Response.Cookies.Delete(Constants.Token);
        
        return TypedResults.Ok(new { message = "Logged out successfully" });
    }

    private static async Task<IResult> PostLogged(ClaimsPrincipal claims, DatabaseContext context)
    {
        var claimId = claims.FindFirst("Sub")?.Value ?? claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = int.Parse(claimId ?? "0");
        var user = await context.UsuariosActivos.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }
        
        return TypedResults.Ok(new GetUsuarioResponse(user.Id, user.Nombre));
    }
}