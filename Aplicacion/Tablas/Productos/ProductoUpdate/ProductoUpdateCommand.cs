using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Productos.ProductoUpdate;
public class ProductoUpdateCommand
{
    public record ProductoUpdateCommandRequest(ProductoUpdateRequest productoUpdateRequest, int ProductoID) : IRequest<Result<int>>;

    internal class ProductoUpdateCommandHandler : IRequestHandler<ProductoUpdateCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;

        public ProductoUpdateCommandHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(
            ProductoUpdateCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            var productoID = request.ProductoID;

            var producto = await _context.productos!
            .FirstOrDefaultAsync(x => x.productoid == productoID);
            
            if (producto is null)
            {
                return Result<int>.Failure("El Producto no existe.");
            }

            producto.descripcion = request.productoUpdateRequest.descripcion!.ToUpper();
            producto.precio = request.productoUpdateRequest.precio;
            producto.categoriaid = request.productoUpdateRequest.categoriaid;
            producto.estado = request.productoUpdateRequest.Estado!.ToUpper();

            if (request.productoUpdateRequest.categoriaid > 0)
            {
                var categoriaa = await _context.categorias!
                .FirstOrDefaultAsync(x => x.categoriaid == request.productoUpdateRequest.categoriaid);

                if (categoriaa is null)
                {
                    return Result<int>.Failure("No se encontro la Categoria.");
                }

                producto.categoria = categoriaa;
            }

            _context.Entry(producto).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(producto.productoid)
                        : Result<int>.Failure("Errores en la actualizacion del Producto.");

        }
    }
    public class ProductoUpdateCommandRequestValidator : AbstractValidator<ProductoUpdateCommandRequest>
    {
        public ProductoUpdateCommandRequestValidator()
        {
            RuleFor(x => x.productoUpdateRequest).SetValidator(new ProductoUpdateValidator());
            RuleFor(x => x.ProductoID).NotNull();
        }
    } 
}
