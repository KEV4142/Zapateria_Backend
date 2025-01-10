using FluentValidation;

namespace Aplicacion.Tablas.Categorias.CategoriaCreate;
public class CategoriaCreateValidator : AbstractValidator<CategoriaCreateRequest>
{
    public CategoriaCreateValidator()
    {
        RuleFor(x => x.Descripcion)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Campo Descripcion es Obligatorio.")
            .NotEmpty().WithMessage("La Descripcion esta en blanco.");
    }
}
