using System.ComponentModel.DataAnnotations;

namespace FactoryHKA.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100)]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [StringLength(150)]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Teléfono inválido")]
    [StringLength(10)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [StringLength(200)]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    [Display(Name = "Fecha de creación")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}