#  Sistema de Ventas — The Factory HKA

Aplicación web backend + frontend para gestión de ventas: selección de cliente, productos y generación de facturas con descuento automático de stock.

> **Evaluación Técnica — Desarrollador I**  
> Desarrollado con ASP.NET Core 8 MVC + SQL Server + Bootstrap 5

---

##  Descripción

El sistema permite:

- Gestionar **clientes** (CRUD completo, 50+ registros de prueba)
-  Gestionar **productos** (CRUD completo, 20+ registros de prueba)
-  **Realizar ventas**: seleccionar cliente, agregar productos, calcular IVA y generar factura
-  **Descuento automático de stock** al confirmar la venta
-  Validaciones de datos con mensajes claros
-  Paginación (20 registros por página)
-  Impresión de facturas

---

##  Tecnologías

| Capa | Tecnología |
|---|---|
| **Backend** | ASP.NET Core 8 MVC (C#) |
| **Frontend** | Razor Views + Bootstrap 5 + Bootstrap Icons |
| **Base de datos** | SQL Server (LocalDB / Express) |
| **ORM** | Entity Framework Core 8 |
| **Pruebas** | xUnit + EF Core InMemory |
| **Interactividad** | JavaScript vanilla (carrito de ventas) |

---

##  Requisitos previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads) o SQL Server LocalDB
- [Visual Studio 2022](https://visualstudio.microsoft.com/) con las cargas de trabajo:
  - "Desarrollo de ASP.NET y web"
  - "Almacenamiento y procesamiento de datos"
- (Opcional) [SQL Server Management Studio](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)

---

##  Instalación y ejecución

### 1. Clonar el repositorio

```bash
git clone https://github.com/carenximem/FactoryHKA.git
cd FactoryHKA
