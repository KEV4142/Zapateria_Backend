using Aplicacion.Tablas.Imagenes;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Interfaces;

public interface IImagenService
{
    Task<ImagenUploadResult> AddImagen(IFormFile file);

    Task<string> DeleteImagen(string publicId);
}
