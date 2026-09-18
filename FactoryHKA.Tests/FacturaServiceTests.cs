using FactoryHKA.Data;
using FactoryHKA.Models;
using FactoryHKA.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace FactoryHKA.Tests;

public class FacturaServiceTests
{
    private ApplicationDbContext GetDb()
    {
        var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var db = new ApplicationDbContext(opts);

        db.Clientes.Add(new Cliente
        {
            Id = 1,
            Nombre = "Test",
            Apellido = "Test",
            Email = "t@t.com",
            Activo = true
        });
        db.Clientes.Add(new Cliente
        {
            Id = 2,
            Nombre = "Inactivo",
            Apellido = "X",
            Email = "x@x.com",
            Activo = false
        });
        db.Productos.Add(new Producto
        {
            Id = 1,
            Nombre = "Producto Test",
            Precio = 100,
            Stock = 10,
            Activo = true
        });
        db.Productos.Add(new Producto
        {
            Id = 2,
            Nombre = "Producto Inactivo",
            Precio = 50,
            Stock = 5,
            Activo = false
        });
        db.SaveChanges();
        return db;
    }

    [Fact]
    public async Task CrearFactura_ConStockSuficiente_DescuentaStockYCalculaTotales()
    {
        var db = GetDb();
        var service = new FacturaService(db);

        var factura = await service.CrearFacturaAsync(1, new() { (1, 3) });

        Assert.Equal(300, factura.Subtotal);
        Assert.Equal(57, factura.Impuesto);
        Assert.Equal(357, factura.Total);
        Assert.Single(factura.Detalles);
        Assert.Equal(7, db.Productos.Find(1)!.Stock);
        Assert.StartsWith("FAC-", factura.NumeroFactura);
    }

    [Fact]
    public async Task CrearFactura_SinStock_LanzaExcepcionYNoDescuenta()
    {
        var db = GetDb();
        var service = new FacturaService(db);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CrearFacturaAsync(1, new() { (1, 999) }));

        Assert.Contains("Stock insuficiente", ex.Message);
        Assert.Equal(10, db.Productos.Find(1)!.Stock);
    }

    [Fact]
    public async Task CrearFactura_ClienteInexistente_LanzaExcepcion()
    {
        var db = GetDb();
        var service = new FacturaService(db);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CrearFacturaAsync(999, new() { (1, 1) }));
    }

    [Fact]
    public async Task CrearFactura_ClienteInactivo_LanzaExcepcion()
    {
        var db = GetDb();
        var service = new FacturaService(db);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CrearFacturaAsync(2, new() { (1, 1) }));

        Assert.Contains("inactivo", ex.Message);
    }

    [Fact]
    public async Task CrearFactura_SinItems_LanzaExcepcion()
    {
        var db = GetDb();
        var service = new FacturaService(db);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CrearFacturaAsync(1, new()));
    }

    [Fact]
    public async Task CrearFactura_ProductoInactivo_LanzaExcepcion()
    {
        var db = GetDb();
        var service = new FacturaService(db);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CrearFacturaAsync(1, new() { (2, 1) }));

        Assert.Contains("inactivo", ex.Message);
    }
}