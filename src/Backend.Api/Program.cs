using Microsoft.OpenApi.Models;
using Backend.Application;
using Backend.Application.Exceptions.Handler;
using Backend.Infrustructure;

namespace Backend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.WebHost.UseUrls("http://0.0.0.0:5001");

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Entertainment API", Version = "v1" });

                // Add JWT authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      //policy.WithOrigins("http://localhost:3000",
                                      //                      "https://localhost:3000",
                                      //                   "http://192.168.152.2:3000",
                                      //                   "https://192.168.152.2:3000",
                                      //                   "http://localhost:5070"
                                      //                   )
                                      policy.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });


            builder.Services.AddExceptionHandler<CustomExceptionHandler>();


            var app = builder.Build();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseExceptionHandler(options => { });
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.MapControllers();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapGet("/", () => "<a ref='localhost:5001/swagger'> click here </a>  ");

            app.Run();
        }
    }
}
