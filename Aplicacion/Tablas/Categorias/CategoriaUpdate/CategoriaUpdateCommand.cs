using Aplicacion.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Tablas.Categorias.CategoriaUpdate;
public class CategoriaUpdateCommand
{
    public record CategoriaUpdateCommandRequest(CategoriaUpdateRequest categoriaUpdateRequest, int CategoriaID):IRequest<Result<int>>;

    internal class CategoriaUpdateCommandHandler
    : IRequestHandler<CategoriaUpdateCommandRequest, Result<int>>
    {
        private readonly BackendContext _context;

        public CategoriaUpdateCommandHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(
            CategoriaUpdateCommandRequest request, 
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

            categoria.descripcion = request.categoriaUpdateRequest.Descripcion!.ToUpper();
            categoria.estado = request.categoriaUpdateRequest.Estado!.ToUpper();

            _context.Entry(categoria).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado 
                        ? Result<int>.Success(categoria.categoriaid)
                        : Result<int>.Failure("Errores en la actualizacion de la Categoria.");

        }
    }
    public class CategoriaUpdateCommandRequestValidator : AbstractValidator<CategoriaUpdateCommandRequest>
    {
        public CategoriaUpdateCommandRequestValidator()
        {
            RuleFor(x => x.categoriaUpdateRequest).SetValidator(new CategoriaUpdateValidator());
            RuleFor(x => x.CategoriaID).NotNull();
        }
    } 
}
