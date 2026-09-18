using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryHKA.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Precio")]
    public decimal Precio { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    [Display(Name = "Stock")]
    public int Stock { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;
}