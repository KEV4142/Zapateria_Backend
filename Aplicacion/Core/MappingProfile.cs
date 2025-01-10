using Aplicacion.Tablas.Categorias.GetCategoria;
using AutoMapper;
using Modelo.Entidades;

namespace Aplicacion.Core;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Categoria, CategoriaResponse>();
    }
}
