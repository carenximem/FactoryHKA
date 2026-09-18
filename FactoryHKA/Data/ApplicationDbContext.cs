using FactoryHKA.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryHKA.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<DetalleFactura> DetalleFacturas => Set<DetalleFactura>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Índices únicos
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email).IsUnique();

        modelBuilder.Entity<Factura>()
            .HasIndex(f => f.NumeroFactura).IsUnique();

        // Relaciones
        modelBuilder.Entity<Factura>()
            .HasOne(f => f.Cliente)
            .WithMany(c => c.Facturas)
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleFactura>()
            .HasOne(d => d.Producto)
            .WithMany()
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Precisión decimal
        modelBuilder.Entity<Producto>().Property(p => p.Precio).HasPrecision(18, 2);
        modelBuilder.Entity<Factura>().Property(f => f.Subtotal).HasPrecision(18, 2);
        modelBuilder.Entity<Factura>().Property(f => f.Impuesto).HasPrecision(18, 2);
        modelBuilder.Entity<Factura>().Property(f => f.Total).HasPrecision(18, 2);
        modelBuilder.Entity<DetalleFactura>().Property(d => d.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<DetalleFactura>().Property(d => d.Subtotal).HasPrecision(18, 2);
    }
}