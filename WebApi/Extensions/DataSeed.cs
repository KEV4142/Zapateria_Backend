using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelo.Entidades;
using Persistencia;
using Persistencia.Models;

namespace WebApi.Extensions;
public static class DataSeed
{
    public static async Task SeedDataAuthentication(
        this IApplicationBuilder app
    )
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try{
            var context = service.GetRequiredService<BackendContext>();
            await context.Database.MigrateAsync();
            var userManager = service.GetRequiredService<UserManager<AppUser>>();

            if(!userManager.Users.Any())
            {
                var userAdmin = new AppUser {
                    NombreCompleto = "Usuario Administrador",
                    UserName = "administrador",
                    Email = "administrador.prueba@gmail.com"
                };

                await userManager.CreateAsync(userAdmin, "Password123$");
                await userManager.AddToRoleAsync(userAdmin, CustomRoles.ADMIN);

                 var userClient = new AppUser {
                    NombreCompleto = "Juan Perez",
                    UserName = "juanperez",
                    Email = "juan.perez@gmail.com"
                };

                await userManager.CreateAsync(userClient, "Password123$");
                await userManager.AddToRoleAsync(userClient, CustomRoles.CLIENT);
            }   

            await context.SaveChangesAsync();
            
        }catch(Exception e)
        {
          var logger = loggerFactory.CreateLogger<BackendContext>();
          logger.LogError(e.Message);
        }


    }
}
