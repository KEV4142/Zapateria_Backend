using FluentValidation;

namespace Aplicacion.Tablas.Categorias.CategoriaUpdate;
public class CategoriaUpdateValidator : AbstractValidator<CategoriaUpdateRequest>
{
    public CategoriaUpdateValidator()
    {
        RuleFor(x => x.Descripcion)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("Campo Descripcion es Obligatorio.")
        .NotEmpty().WithMessage("La Descripcion esta en blanco.");

        RuleFor(x => x.Estado)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("Campo Estado es Obligatorio.")
        .NotEmpty().WithMessage("El campo Estado se encuentra en blanco.")
        .Must(estado => estado == "a" || estado == "b" || estado == "i" || estado == "A" || estado == "B" || estado == "I")
                .WithMessage("El Estado debe ser A, B o I.");
    }
}
