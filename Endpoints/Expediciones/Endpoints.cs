using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Trazert_API.Database;
using Trazert_API.Endpoints.Expediciones.DTOs;

namespace Trazert_API.Endpoints.Expediciones;

public class Endpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/expediciones")
            .WithTags("Expediciones");
        
        group.MapGet("/clientes", async (DatabaseContext context) => await context.Clientes.ToArrayAsync());
        
        group.MapGet("/pedidos", async (DatabaseContext context) => await context.Pedidos.ToArrayAsync());
        
        group.MapGet("/pedidos/{id:int}", async (DatabaseContext context, int id) => 
            await context.PedidosDetalles.Where(p => p.PedidoId == id).ToArrayAsync());

        group.MapGet("/carga/{id:int}", async (DatabaseContext context, int id) => 
            await context.CargaDetalles.Where(c => c.ExpedicionId == id).ToArrayAsync());

        group.MapGet("/despachos/disponibles", async (DatabaseContext context) => 
            await context.DespachosDisponibles.ToArrayAsync());
        
        group.MapPost("/pedido", PostPedido)
            .RequireAuthorization();

        group.MapPost("/despacho", PostDespacho)
            .RequireAuthorization();
    }
    
    private static async Task<IResult> PostPedido(ClaimsPrincipal user, DatabaseContext context, [FromBody] PostPedidoRequest req)
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

            return TypedResults.Ok(new { id = (int)idOut.Value });
        }
        catch (SqlException ex)
        {
            return TypedResults.Conflict(new { message = ex.Message });
        }
    }

    private static async Task<IResult> PostDespacho(ClaimsPrincipal user, DatabaseContext context, [FromBody] PostDespachoRequest req)
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

            return TypedResults.Ok(new { id = (int)idOut.Value });
        }
        catch (SqlException ex)
        {
            return TypedResults.Conflict(new { message = ex.Message });
        }
    }
}