using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Tablas.Productos.ProductoUpdateImagen;
public class ProductoUpdateImagenValidator: AbstractValidator<ProductoUpdateImagenRequest>
{
    public ProductoUpdateImagenValidator()
    {
        RuleFor(x => x.imagenProducto)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("La imagen del producto es obligatoria.")
            .Must(VerificarExtencion).WithMessage("El archivo debe ser una imagen válida (jpg, jpeg, png o webp).")
            .Must(VerificarTamano).WithMessage("El tamaño del archivo no debe exceder 2 MB.");
        
    }
    private bool VerificarExtencion(IFormFile file)
    {
        if (file == null) return false;
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
        return allowedExtensions.Contains(extension);
    }

    private bool VerificarTamano(IFormFile file)
    {
        if (file == null) return false;
        const long maxFileSize = 2 * 1024 * 1024;
        return file.Length <= maxFileSize;
    }
}
