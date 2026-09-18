using FactoryHKA.Data;
using FactoryHKA.Services;
using FactoryHKA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FactoryHKA.Controllers;

public class FacturasController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IFacturaService _facturaService;

    public FacturasController(ApplicationDbContext context, IFacturaService facturaService)
    {
        _context = context;
        _facturaService = facturaService;
    }

    public async Task<IActionResult> Index()
    {
        var facturas = await _facturaService.ListarAsync();
        return View(facturas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var factura = await _facturaService.ObtenerPorIdAsync(id);
        if (factura == null) return NotFound();
        return View(factura);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new CrearFacturaViewModel
        {
            Clientes = await _context.Clientes
                .Where(c => c.Activo)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Nombre} {c.Apellido} - {c.Email}"
                }).ToListAsync(),

            Productos = await _context.Productos
                .Where(p => p.Activo && p.Stock > 0)
                .ToListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CrearFacturaViewModel vm)
    {
        if (vm.ClienteId <= 0)
            ModelState.AddModelError(nameof(vm.ClienteId), "Debe seleccionar un cliente.");

        if (vm.Items == null || !vm.Items.Any())
            ModelState.AddModelError("", "Debe agregar al menos un producto.");

        if (!ModelState.IsValid)
        {
            vm.Clientes = await _context.Clientes
                .Where(c => c.Activo)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Nombre} {c.Apellido} - {c.Email}"
                }).ToListAsync();

            vm.Productos = await _context.Productos
                .Where(p => p.Activo && p.Stock > 0)
                .ToListAsync();

            return View(vm);
        }

        try
        {
            var items = vm.Items.Select(i => (i.ProductoId, i.Cantidad)).ToList();
            var factura = await _facturaService.CrearFacturaAsync(vm.ClienteId, items);
            TempData["Success"] = $"Factura {factura.NumeroFactura} creada exitosamente.";
            return RedirectToAction(nameof(Details), new { id = factura.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);

            vm.Clientes = await _context.Clientes
                .Where(c => c.Activo)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Nombre} {c.Apellido} - {c.Email}"
                }).ToListAsync();

            vm.Productos = await _context.Productos
                .Where(p => p.Activo && p.Stock > 0)
                .ToListAsync();

            return View(vm);
        }
    }
}