using Aplicacion.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Categorias.GetCategoria;
public class GetCategoriaQuery
{
    public record GetCategoriaQueryRequest : IRequest<Result<CategoriaResponse>>
    {
        public int CategoriaID { get; set; }
    }

    internal class GetCategoriaQueryHandler : IRequestHandler<GetCategoriaQueryRequest, Result<CategoriaResponse>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetCategoriaQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<CategoriaResponse>> Handle(
            GetCategoriaQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            var categoria = await _context.categorias!.Where(x => x.categoriaid == request.CategoriaID)
            .ProjectTo<CategoriaResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            if (categoria is null)
            {
                return Result<CategoriaResponse>.Failure("No se encontro la Categoria.");
            }

            return Result<CategoriaResponse>.Success(categoria!);
        }


    }
}
public record CategoriaResponse(
    int categoriaid,
    string descripcion,
    string estado
);