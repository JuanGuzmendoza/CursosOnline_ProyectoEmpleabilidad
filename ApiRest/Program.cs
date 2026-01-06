using CoursesOnline.Domain.Entities;
using CoursesOnline.Extensions; 
using CoursesOnline.Infrastructure; 
using CoursesOnline.Infrastructure.Persistence;
using CoursesOnline.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using CoursesOnline.Application;

var builder = WebApplication.CreateBuilder(args);

// Servicios de Infraestructura y Aplicación
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddHttpClient();

// Configuración de API (Controllers, Identity, JWT, Swagger)
builder.Services.AddApiConfiguration(builder.Configuration);

var app = builder.Build();

// Seeding de la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        await DbSeeder.SeedAsync(services);
        Console.WriteLine("✅ Base de datos inicializada correctamente.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ Error en el Seeding.");
    }
}

// Configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();