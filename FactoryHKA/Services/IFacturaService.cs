using FactoryHKA.Models;

namespace FactoryHKA.Services;

public interface IFacturaService
{
    Task<Factura> CrearFacturaAsync(int clienteId, List<(int productoId, int cantidad)> items);
    Task<Factura?> ObtenerPorIdAsync(int id);
    Task<List<Factura>> ListarAsync();
}