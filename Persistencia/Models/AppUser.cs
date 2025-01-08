using Microsoft.AspNetCore.Identity;

namespace Persistencia.Models;

public class AppUser : IdentityUser
{
    public string? NombreCompleto { get; set; }
}
