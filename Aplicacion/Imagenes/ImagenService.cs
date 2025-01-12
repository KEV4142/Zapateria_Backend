using Aplicacion.Interfaces;
using Aplicacion.Tablas.Imagenes;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Aplicacion.Imagenes;
public class ImagenService : IImagenService
{
    private readonly Cloudinary _cloudinary;

    public ImagenService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<ImagenUploadResult> AddImagen(IFormFile file)
    {
        if(file.Length > 0)
        {

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation()
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if(uploadResult.Error is not null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return new ImagenUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        return null!;
    }

    public async Task<string> DeleteImagen(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result =  await _cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok" ? result.Result! : null!;
    }
}
