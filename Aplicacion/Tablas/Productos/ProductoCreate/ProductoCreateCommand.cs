using Aplicacion.Core;
using Aplicacion.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modelo.Entidades;
using Persistencia;

namespace Aplicacion.Tablas.Productos.ProductoCreate;
public class ProductoCreateCommand
{
    public record ProductoCreateCommandRequest(ProductoCreateRequest productoCreateRequest) : IRequest<Result<int>>;

    internal class ProductoCreateCommandHandler : IRequestHandler<ProductoCreateCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;
        private readonly IImagenService _imagenService;

        public ProductoCreateCommandHandler(
            BackendContext context,
            IImagenService imagenService
            )
        {
            _context = context;
            _imagenService = imagenService;
        }
        
        public async Task<Result<int>> Handle(
            ProductoCreateCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            var producto = new Producto {
                descripcion = request.productoCreateRequest.descripcion!.ToUpper(),
                precio = request.productoCreateRequest.precio,
                categoriaid = request.productoCreateRequest.categoriaid
            };

            if (request.productoCreateRequest.categoriaid > 0)
            {
                var categoriaa = await _context.categorias!
                .FirstOrDefaultAsync(x => x.categoriaid == request.productoCreateRequest.categoriaid);

                if (categoriaa is null)
                {
                    return Result<int>.Failure("No se encontro la Categoria.");
                }

                producto.categoria = categoriaa;
            }

            if(request.productoCreateRequest.imagenProducto is not null)
            {
                var imagenUploadResult = 
                await _imagenService.AddImagen(request.productoCreateRequest.imagenProducto);

                var imagenn = new Imagen
                {
                    url = imagenUploadResult.Url!,
                    publicid = imagenUploadResult.PublicId!,
                };

                producto.imagen = imagenn;
            }



            _context.Add(producto);

            var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
         

            return resultado 
                        ? Result<int>.Success(producto.productoid)
                        : Result<int>.Failure("No se pudo insertar el producto");
            
        }
    }

    public class ProductoCreateCommandRequestValidator
    : AbstractValidator<ProductoCreateCommandRequest>
    {
        public ProductoCreateCommandRequestValidator()
        {
            RuleFor(x => x.productoCreateRequest).SetValidator(new ProductoCreateValidator());
        }

    }
}
