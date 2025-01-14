using Aplicacion.Core;

namespace Aplicacion.Tablas.Productos.GetProductosPagin;
public class GetProductosPaginRequest : PagingParams
{
    public string? Descripcion { get; set; }
    public int CategoriaID { get; set; }
    public string? Estado { get; set; }
}
