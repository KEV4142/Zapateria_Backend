using Aplicacion.Core;
using Aplicacion.Tablas.Productos.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Productos.GetProductosWeb;
public class GetProductosWebQuery
{
    public record GetProductosWebQueryRequest : IRequest<Result<List<ProductoWebResponse>>>
    {
    }
    internal class GetProductosWebQueryHandler
        : IRequestHandler<GetProductosWebQueryRequest, Result<List<ProductoWebResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetProductosWebQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<ProductoWebResponse>>> Handle(
            GetProductosWebQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var productosListado = await _context.productos!
                .Include(p => p.categoria)
                .OrderBy(c => c.categoria.descripcion)
                .Where(p => p.imagenid != null)
                .ProjectTo<ProductoWebResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<ProductoWebResponse>>.Success(productosListado);
        }
    }
}
