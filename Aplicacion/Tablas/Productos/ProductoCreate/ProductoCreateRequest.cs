using Microsoft.AspNetCore.Http;

namespace Aplicacion.Tablas.Productos.ProductoCreate;
public class ProductoCreateRequest
{
    public string? descripcion { get; set; }
    public decimal? precio { get; set; }
    public int categoriaid { get; set; }
    public IFormFile? imagenProducto { get; set; }
}
