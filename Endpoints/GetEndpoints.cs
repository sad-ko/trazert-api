using Microsoft.EntityFrameworkCore;
using Trazert_API.Database;

namespace Trazert_API.Endpoints;

public class GetEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/pedidos", async (DatabaseContext context) => await context.Pedidos.ToArrayAsync())
            .WithName("Pedidos");

        app.MapGet("/clientes", async (DatabaseContext context) => await context.Clientes.ToArrayAsync())
            .WithName("Clientes");

        app.MapGet("/pedidos_detalle", async (DatabaseContext context, int id) =>
            {
                return await context.PedidosDetalles.Where(p => p.PedidoId == id).ToArrayAsync();
            })
            .WithName("Pedidos Detalle");

        app.MapGet("/carga_detalle", async (DatabaseContext context, int id) =>
            {
                return await context.CargaDetalles.Where(c => c.ExpedicionId == id).ToArrayAsync();
            })
            .WithName("Carga Detalle");

        app.MapGet("/despachos_disponibles", async (DatabaseContext context) =>
            {
                await context.DespachosDisponibles.ToArrayAsync();
            })
            .WithName("Despachos Disponibles");
    }
}