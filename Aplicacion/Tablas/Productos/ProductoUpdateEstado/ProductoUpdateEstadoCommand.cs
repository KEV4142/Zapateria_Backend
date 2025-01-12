using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Productos.ProductoUpdateEstado;
public class ProductoUpdateEstadoCommand
{
    public record ProductoUpdateEstadoCommandRequest(ProductoUpdateEstadoRequest productoUpdateEstadoRequest, int ProductoID) : IRequest<Result<int>>;

    internal class ProductoUpdateEstadoCommandHandler : IRequestHandler<ProductoUpdateEstadoCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;

        public ProductoUpdateEstadoCommandHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(
            ProductoUpdateEstadoCommandRequest request, 
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

            producto.estado = request.productoUpdateEstadoRequest.Estado!.ToUpper();

            _context.Entry(producto).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(producto.productoid)
                        : Result<int>.Failure("Errores en la actualizacion del estado del Producto.");

        }
    }
    public class ProductoUpdateEstadoCommandRequestValidator : AbstractValidator<ProductoUpdateEstadoCommandRequest>
    {
        public ProductoUpdateEstadoCommandRequestValidator()
        {
            RuleFor(x => x.productoUpdateEstadoRequest).SetValidator(new ProductoUpdateEstadoValidator());
            RuleFor(x => x.ProductoID).NotNull();
        }
    } 
}
