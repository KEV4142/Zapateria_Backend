using System.Security.Cryptography.X509Certificates;
namespace Aplicacion.Tablas.Imagenes.GetImagen;

public record ImagenResponse(
Guid? Id,
string? Url,
Guid? CursoId)
{
    public ImagenResponse() : this(null, null, null)
    {
    }
}
