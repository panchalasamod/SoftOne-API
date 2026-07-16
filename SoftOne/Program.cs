using Microsoft.EntityFrameworkCore;
using Serilog;
using SoftOne.Data;
using SoftOne.Extensions;
using SoftOne.Middleware;
using SoftOne.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddApplicationServices(builder.Configuration);

    var app = builder.Build();

    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseCors("Frontend");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        //db.Database.Migrate();
        DbInitializer.Seed(db, hasher);
    }

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    throw;
}
finally
{
    Log.CloseAndFlush();
}

// Expose for WebApplicationFactory in integration tests
public partial class Program;
