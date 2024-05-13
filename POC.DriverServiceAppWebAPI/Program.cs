using POC.DriverServiceAppWebAPI.Services;
using Serilog;
using System.Reflection;

namespace POC.DriverServiceAppWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var appSettings = new AppSettings();
            builder.Configuration.GetSection(AppSettings.Section).Bind(appSettings);
            builder.Services.AddSingleton(appSettings);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            builder.Host.UseSerilog(); // Use Serilog for logging

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            builder.Services.AddSingleton<TokenRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapControllers();

            app.Run();
        }
    }

    // todo rename to AuthorizationSettings and move to separate class
    public class AppSettings
    {
        public static string Section = "AppSettings";
        public string CallbackUrl { get; set; }
        public bool SimulateDelay { get; set; }
    }
}
