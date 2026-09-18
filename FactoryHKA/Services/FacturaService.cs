using FactoryHKA.Data;
using FactoryHKA.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryHKA.Services;

public class FacturaService : IFacturaService
{
    private const decimal IVA = 0.19m;
    private readonly ApplicationDbContext _context;

    public FacturaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Factura> CrearFacturaAsync(int clienteId, List<(int productoId, int cantidad)> items)
    {
        var cliente = await _context.Clientes.FindAsync(clienteId)
            ?? throw new InvalidOperationException("El cliente no existe.");

        if (!cliente.Activo)
            throw new InvalidOperationException("El cliente está inactivo.");

        if (items == null || items.Count == 0)
            throw new InvalidOperationException("Debe agregar al menos un producto.");

        foreach (var (_, cantidad) in items)
            if (cantidad <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a 0.");

        using var tx = await _context.Database.BeginTransactionAsync();
        try
        {
            var factura = new Factura
            {
                ClienteId = clienteId,
                Fecha = DateTime.Now,
                NumeroFactura = await GenerarNumeroFacturaAsync(),
                Estado = EstadoFactura.Pagada
            };

            decimal subtotal = 0;

            foreach (var (productoId, cantidad) in items)
            {
                var producto = await _context.Productos.FindAsync(productoId)
                    ?? throw new InvalidOperationException($"Producto {productoId} no existe.");

                if (!producto.Activo)
                    throw new InvalidOperationException($"El producto '{producto.Nombre}' está inactivo.");

                if (producto.Stock < cantidad)
                    throw new InvalidOperationException(
                        $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}");

                var detalle = new DetalleFactura
                {
                    ProductoId = productoId,
                    Cantidad = cantidad,
                    PrecioUnitario = producto.Precio,
                    Subtotal = producto.Precio * cantidad
                };

                subtotal += detalle.Subtotal;
                producto.Stock -= cantidad;
                factura.Detalles.Add(detalle);
            }

            factura.Subtotal = subtotal;
            factura.Impuesto = Math.Round(subtotal * IVA, 2);
            factura.Total = factura.Subtotal + factura.Impuesto;

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return factura;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    private async Task<string> GenerarNumeroFacturaAsync()
    {
        var year = DateTime.Now.Year;
        var count = await _context.Facturas.CountAsync(f => f.Fecha.Year == year);
        return $"FAC-{year}-{(count + 1):D5}";
    }

    public Task<Factura?> ObtenerPorIdAsync(int id) =>
        _context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(f => f.Id == id);

    public Task<List<Factura>> ListarAsync() =>
        _context.Facturas
            .Include(f => f.Cliente)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync();
}