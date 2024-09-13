using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AlbinRonnkvist.HybridSearch.Api;

internal static class ServiceCollectionExtensions
{
    internal static void ConfigureApiProject(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment) 
    {
        services.ConfigureSwagger(configuration, environment);
        services.AddControllers();
        services.ConfigureApiVersioning();
    }

    private static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
        });
    }

    private static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        if (environment.IsDevelopment())
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hybrid Search API",
                    Version = "v1",
                    Description = "Run keyword, semantic and hybrid searches.",
                    Contact = new OpenApiContact
                    {
                        Name = "Albin Rönnkvist",
                        Email = "contact@albinronnkvist.me",
                        Url = new Uri("https://albinronnkvist.me")
                    }
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
