using Aplicacion.Tablas.Categorias.Response;
using Aplicacion.Tablas.Imagenes.GetImagen;

namespace Aplicacion.Tablas.Productos.Response;

public record ProductoResponse(
    int productoid,
    string descripcion,
    decimal precio,
    int categoriaid,
    string estado
);
public record ProductoListaResponse(
    int productoid,
    string descripcion,
    decimal precio,
    int categoriaid,
    int imagenid,
    string estado,
    CategoriaProductoResponse categoria
);
public record ProductoCompletoResponse(
    int productoid,
    string descripcion,
    decimal precio,
    int categoriaid,
    int imagenid,
    string estado,
    CategoriaProductoResponse categoria,
    ImagenResponse? imagen
);

