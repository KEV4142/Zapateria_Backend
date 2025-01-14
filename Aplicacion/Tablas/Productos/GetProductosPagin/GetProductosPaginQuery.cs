using System.Linq.Expressions;
using Aplicacion.Core;
using Aplicacion.Tablas.Productos.GetProducto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Modelo.Entidades;
using Persistencia;

namespace Aplicacion.Tablas.Productos.GetProductosPagin;
public class GetProductosPaginQuery
{
    public record GetProductosPaginQueryRequest : IRequest<Result<PagedList<ProductoResponse>>>
    {
        public GetProductosPaginRequest? ProductosPaginRequest { get; set; }
    }

    internal class GetProductosPaginQueryHandler
    : IRequestHandler<GetProductosPaginQueryRequest, Result<PagedList<ProductoResponse>>>
    {
        private readonly BackendContext _context;
        private readonly IMapper _mapper;

        public GetProductosPaginQueryHandler(BackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ProductoResponse>>> Handle(
            GetProductosPaginQueryRequest request,
            CancellationToken cancellationToken
        )
        {

            IQueryable<Producto> queryable = _context.productos!;

            var predicate = ExpressionBuilder.New<Producto>();
            if (!string.IsNullOrEmpty(request.ProductosPaginRequest!.Descripcion))
            {
                predicate = predicate
                .And(y => y.descripcion!.ToUpper()
                .Contains(request.ProductosPaginRequest.Descripcion.ToUpper()));
            }
            if (!string.IsNullOrEmpty(request.ProductosPaginRequest!.CategoriaID.ToString()) && request.ProductosPaginRequest.CategoriaID != 0)
            {
                predicate = predicate
                .And(y => y.categoriaid==request.ProductosPaginRequest.CategoriaID);
            }
            if (!string.IsNullOrEmpty(request.ProductosPaginRequest!.Estado))
            {
                predicate = predicate
                .And(y => y.estado!.ToUpper()
                .Contains(request.ProductosPaginRequest.Estado.ToUpper()));
            }

            if (!string.IsNullOrEmpty(request.ProductosPaginRequest!.OrderBy))
            {
                Expression<Func<Producto, object>>? orderBySelector =
                                request.ProductosPaginRequest.OrderBy!.ToUpper() switch
                                {
                                    "DESCRIPCION" => producto => producto.descripcion!,
                                    "CATEGORIAID" => producto => producto.categoriaid!,
                                    "ESTADO" => producto => producto.estado!,
                                    _ => producto => producto.productoid!
                                };

                bool orderBy = request.ProductosPaginRequest.OrderAsc ?? true;

                queryable = orderBy
                            ? queryable.OrderBy(orderBySelector)
                            : queryable.OrderByDescending(orderBySelector);
            }
            else
            {
                queryable = queryable.OrderBy(c => c.productoid);
            }

            queryable = queryable.Where(predicate);

            var productosQuery = queryable
            .ProjectTo<ProductoResponse>(_mapper.ConfigurationProvider)
            .AsQueryable();

            var pagination = await PagedList<ProductoResponse>.CreateAsync(
                productosQuery,
                request.ProductosPaginRequest.PageNumber,
                request.ProductosPaginRequest.PageSize
            );

            return Result<PagedList<ProductoResponse>>.Success(pagination);

        }
    }
}
