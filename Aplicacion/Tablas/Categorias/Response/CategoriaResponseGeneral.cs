namespace Aplicacion.Tablas.Categorias.Response;
public record CategoriaResponse(
    int categoriaid,
    string descripcion,
    string estado
);
public record CategoriaProductoResponse(
    string descripcion
);