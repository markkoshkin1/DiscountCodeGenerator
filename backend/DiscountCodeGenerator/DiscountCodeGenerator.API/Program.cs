using DiscountCodeGenerator.API.Interceptors;
using DiscountCodeGenerator.API.Services;
using DiscountCodeGenerator.Db;
using DiscountCodeGenerator.Services.Services.Abstractions;
using DiscountCodeGenerator.Services.Services.Implementations;
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
            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<TimingInterceptor>();
            });

            builder.Services.AddDbContextPool<DiscountCodeContext>(options =>
                             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();
            builder.Services.AddScoped<IDiscountCodeService, DiscountCodeService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DiscountCodeContext>();
                dbContext.Database.Migrate();
            }

            app.MapGrpcService<DiscountService>();

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