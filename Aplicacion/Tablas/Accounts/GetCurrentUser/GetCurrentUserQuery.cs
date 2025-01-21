using Aplicacion.Core;
using Aplicacion.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelo.Entidades;
using Persistencia.Models;

namespace Aplicacion.Tablas.Accounts.GetCurrentUser;
public class GetCurrentUserQuery
{
    public record GetCurrentUserQueryRequest(GetCurrentUserRequest getCurrentUserRequest) : IRequest<Result<Profile>>;

    internal class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQueryRequest, Result<Profile>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public GetCurrentUserQueryHandler(UserManager<AppUser> userManager,ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<Profile>> Handle(
            GetCurrentUserQueryRequest request,
            CancellationToken cancellationToken
            )
        {
            var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == request.getCurrentUserRequest.Email);

            if (user is null)
            {
                return Result<Profile>.Failure("No se encontro el usuario.");
            }
            var tipo="";
            var roleNames = await _userManager.GetRolesAsync(user);
            if (roleNames.Contains(CustomRoles.ADMIN))
            {
                tipo="Administrador";
            }
            else if (roleNames.Contains(CustomRoles.CLIENT))
            {
                tipo="Operador";
            }
            else
            {
                tipo="Sin Asignacion de Rol.";
            }
            var profile = new Profile
            {
                Email = user.Email,
                NombreCompleto = user.NombreCompleto,
                Token = await _tokenService.CreateToken(user),
                Username = user.UserName,
                Tipo=tipo
            };

            return Result<Profile>.Success(profile);
        }
    }
}
