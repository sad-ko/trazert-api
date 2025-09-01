using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Trazert_API.Database;
using Trazert_API.Models.Body;
using Trazert_API.Services;

namespace Trazert_API.Endpoints;

public class PostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/Login", async (HttpContext http, DatabaseContext context, PasswordHasher hasher, TokenProvider provider, [FromBody] LoginRequest request) =>
            {
                var user = await context.UsuariosActivos.FirstAsync(u => u.Nombre == request.Nombre);

                if (user is null || !hasher.Verify(request.Password, user.Hash))
                {
                    return Results.Unauthorized();
                }

                var jwt = provider.Create(user).Result;
                http.Response.Cookies.Append("trazert_jwt", jwt, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                return Results.Ok(new { message = "Logged in!" });
            })
            .WithName("Login")
            .WithOpenApi();

        app.MapPost("/ActualizarExpedicion", async (ClaimsPrincipal user, DatabaseContext context, [FromBody] ActualizarExpedicion req) =>
            {
                try
                {
                    await context.Database.ExecuteSqlRawAsync(
                        "EXEC [dbo].[ActualizarExpedicion] @agregar, @pedido, @codbar, @puesto, @usuario",
                        new SqlParameter("@agregar", req.Agregar),
                        new SqlParameter("@pedido", req.Pedido),
                        new SqlParameter("@codbar", req.Codbar),
                        new SqlParameter("@puesto", 17),
                        new SqlParameter("@usuario", user.FindFirst("Sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    );
                }
                catch (SqlException ex)
                {
                    return Results.Ok(new { message = ex.Message });
                }

                return Results.Ok();
            })
            .WithName("ActualizarExpedicion")
            .WithOpenApi()
            .RequireAuthorization();
    }
}