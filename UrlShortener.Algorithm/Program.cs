using UrlShortener.Algorithm.Services.Implementations;
using UrlShortener.Algorithm.Services.Contracts;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IAlgorithmSettingsService, AlgorithmSettingsService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.Urls.Add("http://*:7047");
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}