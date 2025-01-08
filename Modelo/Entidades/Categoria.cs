namespace Modelo.Entidades;

public partial class Categoria
{
    public int categoriaid { get; set; }

    public string descripcion { get; set; } = null!;

    public string estado { get; set; } = null!;

    public virtual ICollection<Producto> productos { get; set; } = new List<Producto>();
}
