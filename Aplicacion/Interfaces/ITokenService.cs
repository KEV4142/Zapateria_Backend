using Persistencia.Models;

namespace Aplicacion.Interfaces;
public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}
