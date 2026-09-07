namespace Backend.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiService(this IServiceCollection services, string myAllowSpecificOrigins = "_myAllowSpecificOrigins")
        {

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddEndpointsApiExplorer();
            

            services.AddSwaggerGen(c =>
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


            services.AddCors(options =>
            {
                options.AddPolicy(name: myAllowSpecificOrigins,
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


            services.AddExceptionHandler<CustomExceptionHandler>();

            return services;
        }


        public static WebApplication UseApiServices(this WebApplication app, string myAllowSpecificOrigins)
        {
            app.UseCors(myAllowSpecificOrigins);
            app.UseExceptionHandler(options => { });
            app.MapControllers();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapGet("/", () => "<a ref='localhost:5001/swagger'> click here </a>  ");

            return app; 
        }
    }
}
