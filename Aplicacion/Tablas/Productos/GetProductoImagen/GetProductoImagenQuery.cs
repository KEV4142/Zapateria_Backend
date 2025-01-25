using Aplicacion.Core;
using Aplicacion.Tablas.Productos.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Productos.GetProductoImagen;
public class GetProductoImagenQuery
{
    public record GetProductoImagenQueryRequest : IRequest<Result<ProductoImagenResponse>>
    {
        public int ProductoID { get; set; }
    }

    internal class GetProductoImagenQueryHandler : IRequestHandler<GetProductoImagenQueryRequest, Result<ProductoImagenResponse>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetProductoImagenQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ProductoImagenResponse>> Handle(
            GetProductoImagenQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            var producto = await _context.productos!.Where(x => x.productoid == request.ProductoID)
            .ProjectTo<ProductoImagenResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            if (producto is null)
            {
                return Result<ProductoImagenResponse>.Failure("No se encontro el Producto.");
            }

            return Result<ProductoImagenResponse>.Success(producto!);
        }


    }
}
