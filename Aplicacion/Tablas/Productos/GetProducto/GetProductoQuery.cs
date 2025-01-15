using Aplicacion.Core;
using Aplicacion.Tablas.Productos.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;



namespace Aplicacion.Tablas.Productos.GetProducto;
public class GetProductoQuery
{
    public record GetProductoQueryRequest : IRequest<Result<ProductoCompletoResponse>>
    {
        public int ProductoID { get; set; }
    }
    internal class GetProductoQueryHandler : IRequestHandler<GetProductoQueryRequest, Result<ProductoCompletoResponse>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetProductoQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ProductoCompletoResponse>> Handle(
            GetProductoQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            var producto = await _context.productos!.Where(x => x.productoid == request.ProductoID)
            .ProjectTo<ProductoCompletoResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            if (producto is null)
            {
                return Result<ProductoCompletoResponse>.Failure("No se encontro el Producto.");
            }

            return Result<ProductoCompletoResponse>.Success(producto!);
        }


    }
}
