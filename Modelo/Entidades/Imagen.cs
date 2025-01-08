namespace Modelo.Entidades;

public partial class Imagen
{
    public int imagenid { get; set; }

    public string url { get; set; } = null!;

    public string publicid { get; set; } = null!;

    public virtual ICollection<Producto> productos { get; set; } = new List<Producto>();
}
