using Aplicacion.Tablas.Categorias.Response;
using Aplicacion.Tablas.Imagenes.GetImagen;
using Aplicacion.Tablas.Productos.Response;
using AutoMapper;
using Modelo.Entidades;

namespace Aplicacion.Core;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Categoria, CategoriaResponse>();
        CreateMap<Categoria, CategoriaProductoResponse>();
        CreateMap<Producto, ProductoResponse>();
        CreateMap<Producto, ProductoListaResponse>();
        CreateMap<Producto, ProductoCompletoResponse>();
        CreateMap<Imagen, ImagenResponse>();
    }
}
