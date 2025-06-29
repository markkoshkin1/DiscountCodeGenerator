using DiscountCodeGenerator.API.Services;
using DiscountCodeGenerator.Db;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DiscountCodeGenerator.API
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
                                                  .ReadFrom.Configuration(builder.Configuration)
                                                  .CreateLogger();
            builder.Host.UseSerilog();

            // Add services to the container.   
            builder.Services.AddGrpc();
            builder.Services.AddDbContext<DiscountCodeContext>(options =>
                             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DiscountCodeContext>();
                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            app.MapGrpcService<GreeterService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            try
            {
                Log.Information("Starting application");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}