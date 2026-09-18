using FactoryHKA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FactoryHKA.ViewModels;

public class CrearFacturaViewModel
{
    public int ClienteId { get; set; }
    public List<SelectListItem> Clientes { get; set; } = new();
    public List<Producto> Productos { get; set; } = new();
    public List<ItemFacturaViewModel> Items { get; set; } = new();
}

public class ItemFacturaViewModel
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}