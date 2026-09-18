using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryHKA.Models;

public enum EstadoFactura
{
    Pendiente = 0,
    Pagada = 1,
    Anulada = 2
}

public class Factura
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    [Display(Name = "Número de factura")]
    public string NumeroFactura { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Cliente")]
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    [Display(Name = "Fecha")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Subtotal")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "IVA")]
    public decimal Impuesto { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Total")]
    public decimal Total { get; set; }

    [Display(Name = "Estado")]
    public EstadoFactura Estado { get; set; } = EstadoFactura.Pagada;

    public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
}