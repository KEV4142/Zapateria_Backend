using System.Security.Cryptography.X509Certificates;
namespace Aplicacion.Tablas.Imagenes.GetImagen;

public record ImagenResponse(
int? imagenID,
string? Url)
{
    public ImagenResponse() : this(null, null)
    {
    }
}
