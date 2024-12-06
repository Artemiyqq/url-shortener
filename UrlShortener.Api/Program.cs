using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Services.Contracts;
using UrlShortener.Api.Services.Implementations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IAuthService, AuthService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context  = scope.ServiceProvider.GetRequiredService<UrlShortenerDbContext>();

            if (context .Database.GetPendingMigrations().Any())
            {
                context .Database.Migrate();
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(options => options
           .SetIsOriginAllowed(x => _ = true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}