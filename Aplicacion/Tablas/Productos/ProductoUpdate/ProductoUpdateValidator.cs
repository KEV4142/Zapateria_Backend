using FluentValidation;

namespace Aplicacion.Tablas.Productos.ProductoUpdate;
public class ProductoUpdateValidator : AbstractValidator<ProductoUpdateRequest>
{
    public ProductoUpdateValidator()
    {
        RuleFor(x => x.descripcion)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Campo Descripcion es Obligatorio.")
            .NotEmpty().WithMessage("La Descripcion esta en blanco.");

        RuleFor(x => x.precio)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Campo Precio es Obligatorio.")
            .NotEmpty().WithMessage("La Precio esta en blanco.")
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

        RuleFor(x => x.categoriaid)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Campo Categoría es Obligatorio.")
            .NotEmpty().WithMessage("El campo Categoría esta en blanco.")
            .GreaterThan(0).WithMessage("El Campo Categoría es obligatorio.");

        RuleFor(x => x.Estado)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("Campo Estado es Obligatorio.")
        .NotEmpty().WithMessage("El campo Estado se encuentra en blanco.")
        .Must(estado => estado == "a" || estado == "b" || estado == "i" || estado == "A" || estado == "B" || estado == "I")
                .WithMessage("El Estado debe ser A, B o I.");
    }
}
