using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aplicacion.Interfaces;
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

    public TokenService(BackendContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> CreateToken(AppUser usuario)
    {
        var policies = await _context.Database.SqlQuery<string>($@"
                SELECT
                    aspr.ClaimValue
                FROM AspNetUsers a
                    LEFT JOIN AspNetUserRoles ar
                        ON a.Id=ar.UserId
                    LEFT JOIN AspNetRoleClaims aspr
                        ON ar.RoleId = aspr.RoleId
                    WHERE a.Id = {usuario.Id}
        ").ToListAsync();

        var roles = await _context.Database.SqlQuery<string>($@"
            SELECT
                r.Name
            FROM AspNetUserRoles ur
                INNER JOIN AspNetRoles r
                ON ur.RoleId = r.Id
            WHERE ur.UserId = {usuario.Id}
            ").ToListAsync();

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
