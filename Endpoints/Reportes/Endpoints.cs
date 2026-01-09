using Microsoft.EntityFrameworkCore;
using Trazert_API.Database;

namespace Trazert_API.Endpoints.Reportes;

public class Endpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reportes")
            .WithTags("Reportes");

        group.MapGet("/historial_expediciones", async (DatabaseContext context, DateTime? from, DateTime? to) =>
        {
            var query = context
                .HistorialExpediciones
                .AsNoTracking()
                .AsQueryable();

            if (from.HasValue)
            {
                query = query.Where(x => x.FechaHoraCreacion >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(x => x.FechaHoraCreacion <= to.Value);
            }

            return await query.OrderBy(x => x.FechaHoraCreacion).ToArrayAsync();
        });

        group.MapGet("/despacho_detalles/{id:int}", async (DatabaseContext context, int id) =>
        {
            return await context
                .DespachoDetalles
                .Where(x => x.Id == id)
                .ToArrayAsync();
        });
    }
}