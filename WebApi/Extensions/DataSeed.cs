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

        try
        {
            var context = service.GetRequiredService<BackendContext>();
            if (! await TableExistsAsync(context, "AspNetRoles"))
            {
                await context.Database.MigrateAsync();
            }
            var userManager = service.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any())
            {
                var userAdmin = new AppUser
                {
                    NombreCompleto = "Usuario Administrador",
                    UserName = "administrador",
                    Email = "administrador.prueba@gmail.com"
                };

                await userManager.CreateAsync(userAdmin, "Password123$");
                await userManager.AddToRoleAsync(userAdmin, CustomRoles.ADMIN);

                var userClient = new AppUser
                {
                    NombreCompleto = "Juan Perez",
                    UserName = "juanperez",
                    Email = "juan.perez@gmail.com"
                };

                await userManager.CreateAsync(userClient, "Password123$");
                await userManager.AddToRoleAsync(userClient, CustomRoles.CLIENT);
            }

            await context.SaveChangesAsync();

        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<BackendContext>();
            logger.LogError(e.Message);
        }


    }


    private static async Task<bool> TableExistsAsync(BackendContext context, string tableName)
    {
        var query = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = @p0)";
        
        await using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = query;

        var param = command.CreateParameter();
        param.ParameterName = "p0";
        param.Value = tableName;
        command.Parameters.Add(param);

        await context.Database.OpenConnectionAsync();
        var exists = (bool)(await command.ExecuteScalarAsync())!;
        await context.Database.CloseConnectionAsync();

        return exists;
    }
}
