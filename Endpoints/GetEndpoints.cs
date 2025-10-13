using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Trazert_API.Database;
using Trazert_API.Services;

namespace Trazert_API.Endpoints;

public class GetEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/Test", (ClaimsPrincipal user) =>
            {
                return Results.Ok(new { message = $"Hola {user.Identity?.Name}" });
            })
            .RequireAuthorization()
            .WithName("Test")
            .WithOpenApi();

        app.MapGet("/Hash", (PasswordHasher hasher, string password) =>
            {
                return hasher.Hash(password);
            })
            .WithName("Hash")
            .WithOpenApi();

        app.MapGet("/Usuarios", async (DatabaseContext context) =>
            {
                return await context.UsuariosActivos.Select(u => new { u.Id, u.Nombre }).ToArrayAsync();
            })
            .WithName("Usuarios")
            .WithOpenApi();

        app.MapGet("/Pedidos", async (DatabaseContext context) =>
            {
                return await context.Pedidos.ToArrayAsync();
            })
            .WithName("Pedidos")
            .WithOpenApi();

        app.MapGet("/Clientes", async (DatabaseContext context) =>
            {
                return await context.Clientes.ToArrayAsync();
            })
            .WithName("Clientes")
            .WithOpenApi();

        app.MapGet("/PedidosDetalle", async (DatabaseContext context, int id) =>
            {
                return await context.PedidosDetalles.Where(p => p.PedidoId == id).ToArrayAsync();
            })
            .WithName("PedidosDetalle")
            .WithOpenApi();

        app.MapGet("/CargaDetalle", async (DatabaseContext context, int id) =>
            {
                return await context.CargaDetalles.Where(c => c.ExpedicionId == id).ToArrayAsync();
            })
            .WithName("CargaDetalle")
            .WithOpenApi();

        app.MapGet("/DespachosDisponibles", async (DatabaseContext context) =>
            {
                return await context.DespachosDisponibles.ToArrayAsync();
            })
            .WithName("DespachosDisponibles")
            .WithOpenApi();
    }
}