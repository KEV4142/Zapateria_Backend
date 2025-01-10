using FluentValidation;

namespace Aplicacion.Tablas.Categorias.CategoriaUpdateEstado;
public class CategoriaUpdateEstadoValidator : AbstractValidator<CategoriaUpdateEstadoRequest>
{
    public CategoriaUpdateEstadoValidator()
    {
        RuleFor(x => x.Estado)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("Campo Estado es Obligatorio.")
        .NotEmpty().WithMessage("El campo Estado se encuentra en blanco.")
        .Must(estado => estado == "a" || estado == "b" || estado == "i" || estado == "A" || estado == "B" || estado == "I")
                .WithMessage("El Estado debe ser A, B o I.");
    }
}
