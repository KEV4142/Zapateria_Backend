using Aplicacion.Core;

namespace Aplicacion.Tablas.Categorias.GetCategoriasPagin;
public class GetCategoriasPaginRequest : PagingParams
{
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
}
