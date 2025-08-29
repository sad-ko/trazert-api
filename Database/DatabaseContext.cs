using Microsoft.EntityFrameworkCore;
using Trazert_API.Models;

namespace Trazert_API.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Usuario> UsuariosActivos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoDetalles> PedidosDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.Entity<Usuario>(eb =>
        {
            eb.ToView("UsuariosActivos");
        });
        modelBuilder.Entity<Pedido>(eb =>
        {
            eb.ToView("PedidosParaCargar");
        });
        modelBuilder.Entity<PedidoDetalles>(eb =>
        {
            eb.HasNoKey();
            eb.ToView("PedidosDetalles");
        });
    }
}