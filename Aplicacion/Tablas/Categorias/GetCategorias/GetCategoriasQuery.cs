using Aplicacion.Core;
using Aplicacion.Tablas.Categorias.GetCategoria;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Categorias.GetCategorias;
public class GetCategoriasQuery
{
    public record GetCategoriasQueryRequest : IRequest<Result<List<CategoriaResponse>>>
    {
    }

    internal class GetCategoriasQueryHandler
        : IRequestHandler<GetCategoriasQueryRequest, Result<List<CategoriaResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetCategoriasQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<CategoriaResponse>>> Handle(
            GetCategoriasQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var categoriasListado = await _context.categorias!
                .OrderBy(c => c.categoriaid)
                .ProjectTo<CategoriaResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<CategoriaResponse>>.Success(categoriasListado);
        }
    }
}
