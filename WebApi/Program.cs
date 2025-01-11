using Aplicacion;
using Aplicacion.Imagenes;
using Aplicacion.Interfaces;
using DotNetEnv;
using Persistencia;
using WebApi.Extensions;
using WebApi.Middleware;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        { "ConnectionStrings:DefaultConnection", Environment.GetEnvironmentVariable("DB_CONNECTION")! },
        { "TokenKey", Environment.GetEnvironmentVariable("TOKEN_KEY")! },
        { "CloudinarySettings:CloudName", Environment.GetEnvironmentVariable("C_CLOUDNAME")! },
        { "CloudinarySettings:ApiKey", Environment.GetEnvironmentVariable("C_APIKEY")! },
        { "CloudinarySettings:ApiSecret", Environment.GetEnvironmentVariable("C_APISECRET")! }
    
    });


builder.Services.AddApplicacion();
builder.Services.AddPersistencia(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddPoliciesServices();

builder.Services
.Configure<CloudinarySettings>
(builder.Configuration.GetSection(nameof(CloudinarySettings)));

builder.Services.AddScoped<IImagenService, ImagenService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddCors(o => o.AddPolicy("corsapp", builder => {
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.useSwaggerDocumentation();
}

app.UseAuthentication();
app.UseAuthorization();
// await app.SeedDataAuthentication();
app.UseCors("corsapp");
app.MapControllers();


app.Run();

