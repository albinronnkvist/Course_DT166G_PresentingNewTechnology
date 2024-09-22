using AlbinRonnkvist.HybridSearch.Api.Dtos;
using AlbinRonnkvist.HybridSearch.Api.Options;
using AlbinRonnkvist.HybridSearch.Api.Services;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AlbinRonnkvist.HybridSearch.Api;

internal static class ServiceCollectionExtensions
{
    internal const string CorsPolicy = "CorsPolicy";
    internal static void ConfigureApiProject(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment) 
    {
        services.ConfigureSwagger(environment);
        services.AddControllers();
        services.ConfigureApiVersioning();
        services.ConfigureCustomServices();
        services.ConfigureElasticsearch(configuration, environment);
        services.ConfigureCors();
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

    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy(CorsPolicy, builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        });
    }

    private static void ConfigureSwagger(this IServiceCollection services, IHostEnvironment environment)
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

    private static void ConfigureElasticsearch(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<ElasticsearchOptions>(configuration.GetSection(nameof(ElasticsearchOptions)));

        var elasticsearchOptions = configuration.
            GetSection(nameof(ElasticsearchOptions))
            .Get<ElasticsearchOptions>() ?? throw new InvalidOperationException("ElasticsearchOptions is not configured properly.");
        
        var elasticSearchSettings = ConfigureElasticsearchSettings(environment, elasticsearchOptions);

        services.AddSingleton(new ElasticsearchClient(elasticSearchSettings));
    }

    private static ElasticsearchClientSettings ConfigureElasticsearchSettings(IHostEnvironment environment, ElasticsearchOptions elasticsearchOptions)
    {
        if(environment.IsDevelopment())
        {
            return new ElasticsearchClientSettings(new Uri(elasticsearchOptions.Url))
                .DisableDirectStreaming();
        }

        return new ElasticsearchClientSettings(new Uri(elasticsearchOptions.Url))
                    .CertificateFingerprint(elasticsearchOptions.FingerPrint)
                    .Authentication(new BasicAuthentication(elasticsearchOptions.Username, elasticsearchOptions.Password));
    }

    private static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddTransient<ISearcher<ProductSearchResponse>, ProductSearcher>();
    }
}
