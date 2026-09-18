namespace FactoryHKA.ViewModels;

public class PaginacionViewModel<T>
{
    public List<T> Items { get; set; } = new();
    public int PaginaActual { get; set; } = 1;
    public int TamanoPagina { get; set; } = 20;
    public int TotalRegistros { get; set; }

    public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);

    public bool TienePaginaAnterior => PaginaActual > 1;
    public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;

    public int RegistroInicial => TotalRegistros == 0 ? 0 : ((PaginaActual - 1) * TamanoPagina) + 1;
    public int RegistroFinal => Math.Min(PaginaActual * TamanoPagina, TotalRegistros);
}