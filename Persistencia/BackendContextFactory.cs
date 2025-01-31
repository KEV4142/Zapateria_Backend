using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using DotNetEnv;

namespace Persistencia;
public class BackendContextFactory : IDesignTimeDbContextFactory<BackendContext>
{
    public BackendContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../WebApi");

        var envPath = Path.Combine(basePath, ".env");
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
            Console.WriteLine("Variables de entorno cargadas desde .env");
        }
        else
        {
            Console.WriteLine("⚠ Archivo .env no encontrado en: " + envPath);
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("La variable de entorno DB_CONNECTION no está definida.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<BackendContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new BackendContext(optionsBuilder.Options);
    }
}

