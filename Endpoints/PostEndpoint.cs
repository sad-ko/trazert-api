using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Trazert_API.Database;
using Trazert_API.Models.DTOs;

namespace Trazert_API.Endpoints;

public class PostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/actualizar_expedicion_pedido", async (ClaimsPrincipal user, DatabaseContext context, [FromBody] ActualizarExpedicionPorPedido req) =>
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
                    return Results.Conflict(new { message = ex.Message });
                }
            })
            .WithName("Actualizar Expedicion por Pedido")
            .RequireAuthorization();

        app.MapPost("/actualizar_expedicion", async (ClaimsPrincipal user, DatabaseContext context, [FromBody] ActualizarExpedicionSinPedido req) =>
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
                    return Results.Conflict(new { message = ex.Message });
                }
            })
            .WithName("Actualizar Expedicion sin Pedido")
            .RequireAuthorization();
    }
}