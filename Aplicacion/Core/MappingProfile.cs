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
        CreateMap<Categoria, CategoriaWebResponse>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.categoriaid))
            .ForMember(dest => dest.detalle, opt => opt.MapFrom(src => src.descripcion));
        CreateMap<Producto, ProductoResponse>();
        CreateMap<Producto, ProductoListaResponse>();
        CreateMap<Producto, ProductoCompletoResponse>();
        CreateMap<Producto, ProductoWebResponse>()
            .ForMember(dest => dest.detalle, opt => opt.MapFrom(src => src.categoria.descripcion))
            .ForMember(dest => dest.url, opt => opt.MapFrom(src => src.imagen!.url));
        CreateMap<Imagen, ImagenResponse>();
    }
}
