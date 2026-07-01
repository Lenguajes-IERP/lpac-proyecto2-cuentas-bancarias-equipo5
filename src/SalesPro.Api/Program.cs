using SalesPro.Api.Middleware;
using SalesPro.Business.Services;
using SalesPro.Data.Infrastructure;
using SalesPro.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("SalesProDb")
    ?? throw new InvalidOperationException("Falta configurar ConnectionStrings:SalesProDb en appsettings.json.");

builder.Services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

builder.Services.AddScoped<ICatalogoRepository, CatalogoRepository>();
builder.Services.AddScoped<ICuentaBancariaRepository, CuentaBancariaRepository>();
builder.Services.AddScoped<IOrdenRepository, OrdenRepository>();
builder.Services.AddScoped<IParametroSistemaRepository, ParametroSistemaRepository>();

builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<ICuentaBancariaService, CuentaBancariaService>();
builder.Services.AddScoped<IOrdenService, OrdenService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    application = "SalesPro API",
    status = "ok"
}));

app.Run();
