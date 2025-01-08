namespace Modelo.Entidades;

public partial class Producto
{
    public int productoid { get; set; }

    public string? descripcion { get; set; }

    public decimal? precio { get; set; }

    public int categoriaid { get; set; }

    public int? imagenid { get; set; }

    public string estado { get; set; } = null!;

    public virtual Categoria categoria { get; set; } = null!;

    public virtual Imagen? imagen { get; set; }
}
