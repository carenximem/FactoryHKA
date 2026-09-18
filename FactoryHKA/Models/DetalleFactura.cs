using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryHKA.Models;

public class DetalleFactura
{
    public int Id { get; set; }

    public int FacturaId { get; set; }
    public Factura? Factura { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioUnitario { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }
}