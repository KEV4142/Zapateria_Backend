using System.Linq.Expressions;
using Aplicacion.Core;
using Aplicacion.Tablas.Categorias.Response;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Modelo.Entidades;
using Persistencia;

namespace Aplicacion.Tablas.Categorias.GetCategoriasPagin;
public class GetCategoriasPaginQuery
{
    public record GetCategoriasPaginQueryRequest : IRequest<Result<PagedList<CategoriaResponse>>>
    {
        public GetCategoriasPaginRequest? CategoriasPaginRequest { get; set; }
    }

    internal class GetCategoriasPaginQueryHandler
    : IRequestHandler<GetCategoriasPaginQueryRequest, Result<PagedList<CategoriaResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetCategoriasPaginQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<CategoriaResponse>>> Handle(
            GetCategoriasPaginQueryRequest request,
            CancellationToken cancellationToken
        )
        {

            IQueryable<Categoria> queryable = _context.categorias!;

            var predicate = ExpressionBuilder.New<Categoria>();
            if (!string.IsNullOrEmpty(request.CategoriasPaginRequest!.Descripcion))
            {
                predicate = predicate
                .And(y => y.descripcion!.ToUpper()
                .Contains(request.CategoriasPaginRequest.Descripcion.ToUpper()));
            }
            if (!string.IsNullOrEmpty(request.CategoriasPaginRequest!.Estado))
            {
                predicate = predicate
                .And(y => y.estado!.ToUpper()
                .Contains(request.CategoriasPaginRequest.Estado.ToUpper()));
            }

            if (!string.IsNullOrEmpty(request.CategoriasPaginRequest!.OrderBy))
            {
                Expression<Func<Categoria, object>>? orderBySelector =
                                request.CategoriasPaginRequest.OrderBy!.ToUpper() switch
                                {
                                    "DESCRIPCION" => categoria => categoria.descripcion!,
                                    "ESTADO" => categoria => categoria.estado!,
                                    _ => categoria => categoria.categoriaid!
                                };

                bool orderBy = request.CategoriasPaginRequest.OrderAsc ?? true;

                queryable = orderBy
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }
            else
            {
                queryable = queryable.OrderBy(c => c.categoriaid);
            }

            queryable = queryable.Where(predicate);

            var categoriasQuery = queryable
            .ProjectTo<CategoriaResponse>(_mapper.ConfigurationProvider)
            .AsQueryable();

            var pagination = await PagedList<CategoriaResponse>.CreateAsync(
                categoriasQuery,
                request.CategoriasPaginRequest.PageNumber,
                request.CategoriasPaginRequest.PageSize
            );

            return Result<PagedList<CategoriaResponse>>.Success(pagination);

        }
    }
}
