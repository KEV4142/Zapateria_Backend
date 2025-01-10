using Aplicacion.Core;
using Aplicacion.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelo.Entidades;
using Persistencia.Models;

namespace Aplicacion.Tablas.Accounts.UsuarioUpdatePassword;
public class UsuarioUpdatePasswordCommand
{
    public record UsuarioUpdatePasswordCommandRequest(UsuarioUpdatePasswordRequest usuarioUpdatePasswordRequest, string UsuarioID) : IRequest<Result<Profile>>;

    internal class UsuarioUpdatePasswordCommandHandler : IRequestHandler<UsuarioUpdatePasswordCommandRequest, Result<Profile>>
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public UsuarioUpdatePasswordCommandHandler(
            UserManager<AppUser> userManager, 
            ITokenService tokenService,
            IPasswordHasher<AppUser> passwordHasher
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<Profile>> Handle(UsuarioUpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UsuarioID);
            
            if (usuario is null)
            {
                return Result<Profile>.Failure("El Usuario no existe");
            }

            if (request.usuarioUpdatePasswordRequest.Password is not null) {
                usuario.PasswordHash = _passwordHasher.HashPassword(usuario, request.usuarioUpdatePasswordRequest.Password!);
            }
           
            var resultado =  await _userManager.UpdateAsync(usuario);

            if(resultado.Succeeded)
            {
                var tipo="";
                var roleNames = await _userManager.GetRolesAsync(usuario);
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
                    Email = usuario.Email,
                    NombreCompleto = usuario.NombreCompleto,
                    Token = await _tokenService.CreateToken(usuario),
                    Username = usuario.UserName,
                    Tipo=tipo
                };

                return Result<Profile>.Success(profile);
            }

            return Result<Profile>.Failure("Errores en la Actualizacion de la Contraseña.");
        }
    }
    public class UpdatePasswordCommandRequestValidator : AbstractValidator<UsuarioUpdatePasswordCommandRequest>
    {
        public UpdatePasswordCommandRequestValidator()
        {
            RuleFor(x => x.usuarioUpdatePasswordRequest).SetValidator(new UsuarioUpdatePasswordValidator());
            RuleFor(x => x.UsuarioID).NotNull();
        }
    }
}
