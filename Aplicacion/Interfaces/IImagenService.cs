using Aplicacion.Tablas.Imagenes;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Interfaces;

public interface IImagenService
{
    Task<ImagenUploadResult> AddPhoto(IFormFile file);

    Task<string> DeletePhoto(string publicId);
}
