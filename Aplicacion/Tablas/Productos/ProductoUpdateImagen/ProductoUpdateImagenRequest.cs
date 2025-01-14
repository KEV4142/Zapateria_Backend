using Microsoft.AspNetCore.Http;

namespace Aplicacion.Tablas.Productos.ProductoUpdateImagen;
public class ProductoUpdateImagenRequest
{
    public IFormFile imagenProducto { get; set; } = null!;
}
