using DiscountCodeGeneratorClient;

namespace ReactClient.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactDevServer", policy =>
                {
                    policy.WithOrigins("https://localhost:64024")  // React app URL
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Add services to the container.

            builder.Services.AddGrpcClient<Discount.DiscountClient>(o =>
            {
                o.Address = new Uri("https://localhost:7034");  // your gRPC backend address
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            

            var app = builder.Build();

            app.UseDefaultFiles();
            app.MapStaticAssets();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseCors("AllowReactDevServer");
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
