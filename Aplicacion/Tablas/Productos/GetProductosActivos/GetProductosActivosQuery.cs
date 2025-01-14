using Aplicacion.Core;
using Aplicacion.Tablas.Productos.GetProducto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Productos.GetProductosActivos;
public class GetProductosActivosQuery
{
    public record GetProductosActivasQueryRequest : IRequest<Result<List<ProductoResponse>>>
    {
    }
    internal class GetProductosActivasQueryHandler
        : IRequestHandler<GetProductosActivasQueryRequest, Result<List<ProductoResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetProductosActivasQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<ProductoResponse>>> Handle(
            GetProductosActivasQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var productoListado = await _context.productos!
                .Where(s => s.estado!=null && s.estado.ToUpper().Equals("A"))
                .OrderBy(c => c.productoid)
                .ProjectTo<ProductoResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<ProductoResponse>>.Success(productoListado);
        }
    }
}
