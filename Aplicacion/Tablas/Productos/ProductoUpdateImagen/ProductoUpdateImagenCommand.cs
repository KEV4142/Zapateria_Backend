using Aplicacion.Core;
using Aplicacion.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modelo.Entidades;
using Persistencia;

namespace Aplicacion.Tablas.Productos.ProductoUpdateImagen;
public class ProductoUpdateImagenCommand
{
    public record ProductoUpdateImagenCommandRequest(ProductoUpdateImagenRequest productoUpdateImagenRequest, int ProductoID) : IRequest<Result<int>>;

    internal class ProductoUpdateImagenCommandHandler : IRequestHandler<ProductoUpdateImagenCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;
        private readonly IImagenService _imagenService;

        public ProductoUpdateImagenCommandHandler(BackendContext context, IImagenService imagenService)
        {
            _context = context;
            _imagenService = imagenService;
        }

        public async Task<Result<int>> Handle(
            ProductoUpdateImagenCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            var productoID = request.ProductoID;

            var producto = await _context.productos!
            .Include(p => p.imagen)
            .FirstOrDefaultAsync(x => x.productoid == productoID);
            
            if (producto is null)
            {
                return Result<int>.Failure("El Producto no existe.");
            }

            

            if(producto.imagenid is not null)
            {
                var imagenResult = 
                await _imagenService.DeleteImagen(producto.imagen!.publicid);
                producto.imagen.estado="I";

                var imagenUploadResult = 
                await _imagenService.AddImagen(request.productoUpdateImagenRequest.imagenProducto);
                var imagenProducto = new Imagen
                {
                    url = imagenUploadResult.Url!,
                    publicid = imagenUploadResult.PublicId!,
                };

                producto.imagen = imagenProducto;
            }
            else{
                var imagenUploadResult = 
                await _imagenService.AddImagen(request.productoUpdateImagenRequest.imagenProducto);
                var imagenProducto = new Imagen
                {
                    url = imagenUploadResult.Url!,
                    publicid = imagenUploadResult.PublicId!,
                };

                producto.imagen = imagenProducto;
            }

            _context.Entry(producto).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(producto.productoid)
                        : Result<int>.Failure("Errores en la actualizacion del Producto."); 

        }
    }
    public class ProductoUpdateImagenCommandRequestValidator : AbstractValidator<ProductoUpdateImagenCommandRequest>
    {
        public ProductoUpdateImagenCommandRequestValidator()
        {
            RuleFor(x => x.productoUpdateImagenRequest).SetValidator(new ProductoUpdateImagenValidator());
            RuleFor(x => x.ProductoID).NotNull().WithMessage("No envio el ID del Producto.");
        }
    } 
}
