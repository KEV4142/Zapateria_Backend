using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modelo.Entidades;
using Persistencia;
using Persistencia.Models;

namespace Aplicacion.Security;
public class TokenService : ITokenService
{
    private readonly BackendContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(BackendContext context, IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> CreateToken(AppUser usuario)
    {
        var roles = await _userManager.GetRolesAsync(usuario);

        var policies = await (from rc in _context.RoleClaims
                      join r in _context.Roles on rc.RoleId equals r.Id into roleGroup
                      from role in roleGroup.DefaultIfEmpty()
                      where role != null && roles.Contains(role.Name!)
                      select rc.ClaimValue).ToListAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.UserName!),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id),
            new Claim(ClaimTypes.Email, usuario.Email!)
        };

        foreach (var policy in policies)
        {
            if (policy is not null)
            {
                claims.Add(new(CustomClaims.POLICIES, policy));
            }
        }

        foreach (var role in roles)
        {
            if (role is not null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]!)),
            SecurityAlgorithms.HmacSha256
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
