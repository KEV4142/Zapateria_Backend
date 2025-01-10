using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Categorias.CategoriaUpdateEstado;
public class CategoriaUpdateEstadoCommand
{
    public record CategoriaUpdateEstadoCommandRequest(CategoriaUpdateEstadoRequest categoriaUpdateEstadoRequest, int CategoriaID):IRequest<Result<int>>;

    internal class CategoriaUpdateEstadoCommandHandler
    : IRequestHandler<CategoriaUpdateEstadoCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;

        public CategoriaUpdateEstadoCommandHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(
            CategoriaUpdateEstadoCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            var categoriaID = request.CategoriaID;

            var categoria = await _context.categorias!
            .FirstOrDefaultAsync(x => x.categoriaid == categoriaID);
            
            if (categoria is null)
            {
                return Result<int>.Failure("La Categoria no existe.");
            }

            categoria.estado = request.categoriaUpdateEstadoRequest.Estado!.ToUpper();

            _context.Entry(categoria).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(categoria.categoriaid)
                        : Result<int>.Failure("Errores al Actualizar Estado de la Categoria.");

        }
    }
    public class CategoriaUpdateEstadoCommandRequestValidator : AbstractValidator<CategoriaUpdateEstadoCommandRequest>
    {
        public CategoriaUpdateEstadoCommandRequestValidator()
        {
            RuleFor(x => x.categoriaUpdateEstadoRequest).SetValidator(new CategoriaUpdateEstadoValidator());
            RuleFor(x => x.CategoriaID).NotNull().WithMessage("Campo ID es Obligatorio.");
        }
    } 
}
