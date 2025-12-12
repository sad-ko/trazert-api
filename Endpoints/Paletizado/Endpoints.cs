using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Trazert_API.Database;
using Trazert_API.Endpoints.Paletizado.DTOs;

namespace Trazert_API.Endpoints.Paletizado;

public class Endpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/paletizado")
            .WithTags("Paletizado");
        
        group.MapGet("/", async (DatabaseContext context) => await context.ListarPallets.ToArrayAsync());

        group.MapGet("/{id:int}", async (DatabaseContext context, int id) =>
        {
            var codbar = await context.Database.SqlQuery<string>($"SELECT [dbo].[ObtenerCodBarPallet]({id})").ToArrayAsync();
            return TypedResults.Ok(codbar[0]);
        });
        
        group.MapGet("/{codbar}", async (DatabaseContext context, string codbar) =>
        {
            var id = await context.Database.SqlQuery<int>($"SELECT [dbo].[ObtenerIdPallet]({codbar})").ToArrayAsync();
            return TypedResults.Ok(id[0]);
        });

        group.MapPost("/crear", CreatePalet)
            .RequireAuthorization();

        group.MapPost("/cerrar", ClosePalet)
            .RequireAuthorization();

        group.MapPost("/reabrir", OpenPalet)
            .RequireAuthorization();
        
        group.MapDelete("/{id:int}", DeletePalet)
            .RequireAuthorization();

        group.MapPost("/{id:int}/add", AddToPalet)
            .RequireAuthorization();
        
        group.MapPost("/{id:int}/remove", RemoveFromPalet)
            .RequireAuthorization();
    }
    
    private static async Task<IResult> CreatePalet(ClaimsPrincipal user, DatabaseContext context, [FromBody] CreatePalletRequest req)
    {
        try
        {
            var paletId = new SqlParameter
            {
                ParameterName = "@Retorno",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            await context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[PalletCrear] @monoProducto, @idPuesto, @idUsuario, @Retorno OUT",
                new SqlParameter("@monoProducto", req.MonoProducto),
                new SqlParameter("@idPuesto", 17),
                new SqlParameter("@idUsuario", user.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                paletId
            );
            
            return TypedResults.Ok(new { id = (int)paletId.Value });
        }
        catch (SqlException ex)
        {
            return TypedResults.Conflict(new { message = ex.Message });
        }
    }

    private static async Task<IResult> ClosePalet(ClaimsPrincipal user, DatabaseContext context, [FromBody] ClosePalletRequest req)
    {
        try
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[PalletCerrar] @idPallet, @idPuesto, @idUsuario",
                new SqlParameter("@idPallet", req.Id),
                new SqlParameter("@idPuesto", 17),
                new SqlParameter("@idUsuario", user.FindFirst("Sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            );
            
            return TypedResults.Ok();
        }
        catch (SqlException ex)
        {
            return TypedResults.Conflict(new { message = ex.Message });
        }
    }
    
    private static async Task<IResult> OpenPalet(ClaimsPrincipal user, DatabaseContext context, [FromBody] OpenPalletRequest req)
    {
        try
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[PalletReAbrir] @idPallet",
                new SqlParameter("@idPallet", req.Id)
            );
            
            return TypedResults.Ok();
        }
        catch (SqlException ex)
        {
            return TypedResults.Conflict(new { message = ex.Message });
        }
    }
    
    private static async Task<IResult> DeletePalet(ClaimsPrincipal user, DatabaseContext context, int id)
    {
        try
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[PalletEliminar] @idPallet, @idPuesto, @idUsuario",
                new SqlParameter("@idPallet", id),
                new SqlParameter("@idPuesto", 17),
                new SqlParameter("@idUsuario", user.FindFirst("Sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            );
            
            return TypedResults.Ok();
        }
        catch (SqlException ex)
        {
            return TypedResults.Conflict(new { message = ex.Message });
        }
    }
    
    private static async Task<IResult> AddToPalet(ClaimsPrincipal user, DatabaseContext context, int id, [FromBody] PostPalletRequest req)
    {
        try
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[PalletAgregarQuitar] @agregar, @idPallet, @codbar, @idPuesto, @idUsuario",
                new SqlParameter("@agregar", true),
                new SqlParameter("@idPallet", id),
                new SqlParameter("@codbar", req.Codbar),
                new SqlParameter("@idPuesto", 17),
                new SqlParameter("@idUsuario", user.FindFirst("Sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            );
            
            return TypedResults.Ok();
        }
        catch (SqlException ex)
        {
            return TypedResults.Conflict(new { message = ex.Message });
        }
    }
    
    private static async Task<IResult> RemoveFromPalet(ClaimsPrincipal user, DatabaseContext context, int id, [FromBody] PostPalletRequest req)
    {
        try
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[PalletAgregarQuitar] @agregar, @idPallet, @codbar, @idPuesto, @idUsuario",
                new SqlParameter("@agregar", false),
                new SqlParameter("@idPallet", id),
                new SqlParameter("@codbar", req.Codbar),
                new SqlParameter("@idPuesto", 17),
                new SqlParameter("@idUsuario", user.FindFirst("Sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            );
            
            return TypedResults.Ok();
        }
        catch (SqlException ex)
        {
            return TypedResults.Conflict(new { message = ex.Message });
        }
    }
    
}