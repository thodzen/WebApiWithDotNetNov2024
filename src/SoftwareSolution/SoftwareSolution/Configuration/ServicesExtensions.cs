using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace HtTemplate.Configuration;
public static class ServicesExtensions
{
    public static WebApplicationBuilder AddCustomFeatureManagement(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApiFeatureManagementOptions>(
            builder.Configuration.GetSection(ApiFeatureManagementOptions.FeatureManagement));
        builder.Services.AddFeatureManagement();
        return builder;
    }
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(DefaultJsonOptions.Configure);
        services.AddAuthentication().AddJwtBearer();
        services.AddFeatureManagement();
        services.AddSingleton(() => TimeProvider.System);
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddCustomOasGeneration(this IServiceCollection services)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(
            options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header with bearer token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            Scheme = "oauth2",
                            Name = "Bearer ",
                            In = ParameterLocation.Header
                        },
                        []
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            });
        services.AddFluentValidationRulesToSwagger();
        return services;
    }
}