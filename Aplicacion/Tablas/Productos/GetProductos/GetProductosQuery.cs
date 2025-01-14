using Aplicacion.Core;
using Aplicacion.Tablas.Productos.GetProducto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Productos.GetProductos;
public class GetProductosQuery
{
    public record GetProductosQueryRequest : IRequest<Result<List<ProductoResponse>>>
    {
    }
    internal class GetProductosQueryHandler
        : IRequestHandler<GetProductosQueryRequest, Result<List<ProductoResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetProductosQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<ProductoResponse>>> Handle(
            GetProductosQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var productosListado = await _context.productos!
                .OrderBy(c => c.productoid)
                .ProjectTo<ProductoResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<ProductoResponse>>.Success(productosListado);
        }
    }
}
