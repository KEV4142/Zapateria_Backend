using Aplicacion.Core;
using Aplicacion.Tablas.Categorias.GetCategoria;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Categorias.GetCategoriasActivos;
public class GetCategoriasActivosQuery
{
    public record GetCategoriasActivasQueryRequest : IRequest<Result<List<CategoriaResponse>>>
    {
    }
    internal class GetCategoriasActivasQueryHandler
        : IRequestHandler<GetCategoriasActivasQueryRequest, Result<List<CategoriaResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetCategoriasActivasQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<CategoriaResponse>>> Handle(
            GetCategoriasActivasQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var categoriasListado = await _context.categorias!
                .Where(s => s.estado!=null && s.estado.ToUpper().Equals("A"))
                .OrderBy(c => c.categoriaid)
                .ProjectTo<CategoriaResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<CategoriaResponse>>.Success(categoriasListado);
        }
    }
}
