using POC.DriverServiceAppWebAPI.Services;
using Serilog;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;

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
            builder.Services.AddSingleton<WebSocketConnectionsService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseWebSockets();
            app.Map("ws/token", async context =>
            {
                var connectionsService = context.RequestServices.GetRequiredService<WebSocketConnectionsService>();
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var ws = await context.WebSockets.AcceptWebSocketAsync();
                    var clientId = context.Request.Query["clientId"];
                    connectionsService.AddConnection(clientId, ws);
                    
                    await ReceiveMessageAsync(ws, async (result, buffer) =>
                    {
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            // todo
                            await Console.Out.WriteLineAsync(Encoding.UTF8.GetString(buffer));
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            connectionsService.RemoveConnection(clientId);
                        }
                    });
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapControllers();

            app.Run();
        }

        private static async Task ReceiveMessageAsync(WebSocket connection, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[4096];
            while (connection.State == WebSocketState.Open)
            {
                var result = await connection.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }
        }
    }

    // todo rename to AuthorizationSettings and move to separate class
    public class AppSettings
    {
        public static string Section = "AppSettings";
        public string CallbackUrlLp { get; set; }
        public string CallbackUrlWs { get; set; }
        public bool SimulateDelay { get; set; }
    }
}
