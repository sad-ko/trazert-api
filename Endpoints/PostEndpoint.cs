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

        app.MapPost("/ActualizarExpedicionPorPedido", async (ClaimsPrincipal user, DatabaseContext context, [FromBody] ActualizarExpedicionPorPedido req) =>
            {
                try
                {
                    var idOut = new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    await context.Database.ExecuteSqlRawAsync(
                        "EXEC [dbo].[ActualizarExpedicionPorPedido] @agregar, @pedido, @codbar, @puesto, @usuario, @id OUT",
                        new SqlParameter("@agregar", req.Agregar),
                        new SqlParameter("@pedido", req.Pedido),
                        new SqlParameter("@codbar", req.Codbar),
                        new SqlParameter("@puesto", 17),
                        new SqlParameter("@usuario", user.FindFirst("Sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                        idOut
                    );

                    return Results.Ok(new { id = (int)idOut.Value });
                }
                catch (SqlException ex)
                {
                    return Results.Ok(new { message = ex.Message });
                }
            })
            .WithName("ActualizarExpedicionPorPedido")
            .WithOpenApi()
            .RequireAuthorization();

        app.MapPost("/ActualizarExpedicionSinPedido", async (ClaimsPrincipal user, DatabaseContext context, [FromBody] ActualizarExpedicionSinPedido req) =>
            {
                try
                {
                    var idOut = new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    await context.Database.ExecuteSqlRawAsync(
                        "EXEC [dbo].[ActualizarExpedicionSinPedido] @agregar, @expedicion_id, @idCtaAuxi, @codbar, @puesto, @usuario, @id OUT",
                        new SqlParameter("@agregar", req.Agregar),
                        new SqlParameter("@expedicion_id", req.Expedicion ?? (object)DBNull.Value),
                        new SqlParameter("@idCtaAuxi", req.CtaAuxi),
                        new SqlParameter("@codbar", req.Codbar),
                        new SqlParameter("@puesto", 17),
                        new SqlParameter("@usuario", user.FindFirst("Sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                        idOut
                    );

                    return Results.Ok(new { id = (int)idOut.Value });
                }
                catch (SqlException ex)
                {
                    return Results.Ok(new { message = ex.Message });
                }
            })
            .WithName("ActualizarExpedicionSinPedido")
            .WithOpenApi()
            .RequireAuthorization();
    }
}