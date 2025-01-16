using Aplicacion.Core;
using Aplicacion.Tablas.Categorias.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Categorias.GetCategoriasWeb;
public class GetCategoriasWebQuery
{
    public record GetCategoriasWebQueryRequest : IRequest<Result<List<CategoriaWebResponse>>>
    {
    }

    internal class GetCategoriasWebQueryHandler
        : IRequestHandler<GetCategoriasWebQueryRequest, Result<List<CategoriaWebResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetCategoriasWebQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<CategoriaWebResponse>>> Handle(
            GetCategoriasWebQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var categoriasListado = await _context.categorias!
                .OrderBy(c => c.descripcion)
                .ProjectTo<CategoriaWebResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<CategoriaWebResponse>>.Success(categoriasListado);
        }
    }
}
