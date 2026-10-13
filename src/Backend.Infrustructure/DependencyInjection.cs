using Backend.Application.Interfaces.Frames;
using Backend.Infrustructure.Services.Frames;
using Backend.Infrustructure.Services.RabbitMq;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Backend.Infrustructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("Postgres connection missing");

            services.AddDbContext<ApplicationDbContext>( (sp, options) =>
            {
                //options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>() );
                options.UseNpgsql(connectionString);

            });


            services.Configure<RabbitMqOptions>(
                    configuration.GetSection(RabbitMqOptions.SectionName));
            services.Configure<RedisOptions>(
                    configuration.GetSection(RedisOptions.SectionName));


            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisOpts = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
                var options = ConfigurationOptions.Parse(redisOpts.BuildConnectionString());
                return ConnectionMultiplexer.Connect(options);
            });
            
            // Register the distributed cache with empty options...
            services.AddStackExchangeRedisCache(_ => { });

            // ...then configure it from the DI-bound RedisOptions.
            services.AddOptions<RedisCacheOptions>()
                .Configure<IOptions<RedisOptions>>((cacheOptions, redisOptions) =>
                {
                    cacheOptions.Configuration = redisOptions.Value.BuildConnectionString();
                });

            // Identity configuration

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); ;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"])
                    )

                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var claims = context.Principal.Claims;
                        var tokenTypeClaim = claims.FirstOrDefault(c => c.Type == "token_type")?.Value;

                        // Check if the token type is "access_token"
                        if (tokenTypeClaim != "access_token")
                        {
                            context.Fail("Unauthorized"); // Reject the token if it's not an access token
                        }

                        return Task.CompletedTask;
                    }
                };


            });




            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<IDoorRepository, DoorRepository>();
            services.AddScoped<ICameraRepository, CameraRepository>();
            services.AddScoped<IMediaProbeService, FFmpegMediaProbeService>();
            services.AddScoped<IMediaService, MediaService>();


            services.AddSingleton<IFFmpegProcessFactory, FFmpegProcessFactory>();
            services.AddSingleton<IErrorHandler, FFmpegErrorHandler>();
            services.AddScoped<IFrameBufferProcessor, FrameBufferProcessor>();
            services.AddSingleton<ITaskConfigManager, TaskConfigManager>();
            services.AddScoped<IFrameExtractorService, FrameExtractorService>();

            services.AddSingleton<FrameChannel>();
            services.AddSingleton<IFramePublisher>(sp => sp.GetRequiredService<FrameChannel>());
            services.AddHostedService<FramePublisherService>();
            services.AddSingleton<IRawFrameStore, RedisRawFrameStore>();
            services.AddSingleton<IFrameStateStore, FrameSignalStore>();


            services.AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>();
            services.AddSingleton<IRabbitMqTopology, RabbitMqTopology>();
            services.AddSingleton<IFrameSignalPublisher, RabbitMqFrameSignalPublisher>();
            services.AddSingleton<IRecognitionSignalPublisher, RabbitMqRecognitionSignalPublisher>();
            services.AddHostedService<RabbitMqTopologyInitializer>();

            services.Decorate<IAccountRepository, AccountCache>();

            return services;
        }
    }
}
