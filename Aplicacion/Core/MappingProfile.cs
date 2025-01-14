using Aplicacion.Tablas.Categorias.GetCategoria;
using Aplicacion.Tablas.Productos.GetProducto;
using AutoMapper;
using Modelo.Entidades;

namespace Aplicacion.Core;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Categoria, CategoriaResponse>();
        CreateMap<Producto, ProductoResponse>();
    }
}
