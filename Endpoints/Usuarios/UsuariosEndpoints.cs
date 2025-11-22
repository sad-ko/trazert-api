using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trazert_API.Database;
using Trazert_API.Endpoints.Usuarios.DTOs;
using Trazert_API.Services;

namespace Trazert_API.Endpoints.Usuarios;

public class UsuariosEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/usuarios")
            .WithTags("Usuarios");
        
        group.MapGet("/hash", (PasswordHasher hasher, string password) => hasher.Hash(password));

        group.MapGet("/usuarios", GetUsuarios);
        
        group.MapPost("/login", PostLogin);
    }

    private static async Task<GetUsuarioResponse[]> GetUsuarios(DatabaseContext context)
    {
        return await context.UsuariosActivos.Select(u => new GetUsuarioResponse(u.Id, u.Nombre)).ToArrayAsync();
    }

    private static async Task<IResult> PostLogin(HttpContext http, DatabaseContext context, PasswordHasher hasher, TokenProvider provider, [FromBody] PostLoginRequest request)
    {
        var user = await context.UsuariosActivos.FirstOrDefaultAsync(u => u.Nombre == request.Nombre);

        if (user is null || !hasher.Verify(request.Password, user.Hash))
        {
            return TypedResults.Unauthorized();
        }

        var jwt = provider.Create(user).Result;
        http.Response.Cookies.Append("trazert_jwt", jwt, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddHours(1)
        });

        return TypedResults.Ok();
    }
}