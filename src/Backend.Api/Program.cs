

namespace Backend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddInfrastructureServices(builder.Configuration).AddApplicationServices().AddApiService(myAllowSpecificOrigins);


            builder.WebHost.UseUrls("http://0.0.0.0:5001");



            var app = builder.Build();

            app.UseApiServices(myAllowSpecificOrigins);


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
