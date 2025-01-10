using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Modelo.Entidades;
using Persistencia;

namespace Aplicacion.Tablas.Categorias.CategoriaCreate;
public class CategoriaCreateCommand
{
    public record CategoriaCreateCommandRequest(CategoriaCreateRequest categoriaCreateRequest) : IRequest<Result<int>>;

    internal class CategoriaCreateHandler : IRequestHandler<CategoriaCreateCommandRequest, Result<int>>
    {
        private readonly BackendContext _backendContext;
        public CategoriaCreateHandler(BackendContext backendContext)
        {
            _backendContext = backendContext;
        }
        public async Task<Result<int>> Handle(CategoriaCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var categoria = new Categoria
            {
                descripcion = request.categoriaCreateRequest.Descripcion!.ToUpper()
            };
            
            _backendContext.Add(categoria);
            var resultado = await _backendContext.SaveChangesAsync(cancellationToken)> 0;
            return resultado 
                        ? Result<int>.Success(categoria.categoriaid)
                        : Result<int>.Failure("No se pudo insertar la Categoria.");
        }
    }

    public class CategoriaCreateCommandRequestValidator:AbstractValidator<CategoriaCreateCommandRequest>
    {
        public CategoriaCreateCommandRequestValidator()
        {
            RuleFor(x=>x.categoriaCreateRequest).SetValidator(new CategoriaCreateValidator());
        }
    }
}
