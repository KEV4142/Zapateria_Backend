namespace Aplicacion.Tablas.Productos.ProductoUpdate;
public class ProductoUpdateRequest
{
    public string? descripcion { get; set; }
    public decimal? precio { get; set; }
    public int categoriaid { get; set; }
    public string? Estado { get; set; }
}
