using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UJCV_IPP.Data;
using UJCV_IPP.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura el contexto de la base de datos para usar SQL Server.
builder.Services.AddDbContext<UJCV_IPPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UJCV_IPPContext") ?? throw new InvalidOperationException("Connection string 'UJCV_IPPContext' not found.")));

// Agrega servicios al contenedor.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Inicializa los datos de la base de datos.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configura el pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor predeterminado de HSTS es de 30 días. Puede que desees cambiar esto para escenarios de producción.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Mapea la ruta del controlador.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
