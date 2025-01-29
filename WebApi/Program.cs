using System.Globalization;
using Aplicacion;
using Aplicacion.Imagenes;
using Aplicacion.Interfaces;
using DotNetEnv;
using Persistencia;
using WebApi.Extensions;
using WebApi.Middleware;

Env.Load();
// var builder = WebApplication.CreateBuilder(args);

var basePath = AppContext.BaseDirectory;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = basePath,
    Args = args
});

builder.Configuration
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        { "ConnectionStrings:DefaultConnection", Environment.GetEnvironmentVariable("DB_CONNECTION")! },
        { "TokenKey", Environment.GetEnvironmentVariable("TOKEN_KEY")! },
        { "CloudinarySettings:CloudName", Environment.GetEnvironmentVariable("C_CLOUDNAME")! },
        { "CloudinarySettings:ApiKey", Environment.GetEnvironmentVariable("C_APIKEY")! },
        { "CloudinarySettings:ApiSecret", Environment.GetEnvironmentVariable("C_APISECRET")! },
        { "AllowedHosts", Environment.GetEnvironmentVariable("BACKEND_ORIGIN")! },
        { "AllowedFrontendHosts", Environment.GetEnvironmentVariable("FRONTEND_ORIGIN")! }
    });

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddApplicacion();
builder.Services.AddPersistencia(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddPoliciesServices();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));
builder.Services.AddScoped<IImagenService, ImagenService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();


var allowedFrontendOrigins = builder.Configuration.GetSection("AllowedFrontendHosts").Get<string>();
builder.Services.AddCors(o => o.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins(allowedFrontendOrigins!).AllowAnyMethod().AllowAnyHeader();
}));
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.useSwaggerDocumentation();
}

app.UseAuthentication();
app.UseAuthorization();
// app.UseHttpsRedirection();

await app.SeedDataAuthentication();
app.UseCors("corsapp");
app.MapControllers();


app.Run();

