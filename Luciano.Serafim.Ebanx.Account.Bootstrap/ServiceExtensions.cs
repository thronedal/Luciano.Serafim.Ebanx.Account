using System;
using System.Reflection;
using System.Text.Json.Serialization;
using Luciano.Serafim.Ebanx.Account.Bootstrap.Filters;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap;

public static class ServiceExtensions
{
    public static IServiceCollection AddEbanxMediatR(this IServiceCollection services)
    {
        Assembly[] assemblies = [Assembly.Load("Luciano.Serafim.Ebanx.Account.Core")];
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
        });
        return services;
    }

    public static IServiceCollection AddEbanxDistributedCache(this IServiceCollection services)
    {
        //use memcache for testing
        //TODO: substitute for redis or another cache server for production
        services.AddDistributedMemoryCache();

        return services;
    }

    public static IServiceCollection AddEbanxControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            //register filters globally
            options.Filters.Add<ExceptionFilter>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }

    public static IServiceCollection AddEbanxHttpLogging(this IServiceCollection services)
    {
        //configure Http Logging (https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging)
        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            //logging.RequestHeaders.Add("sec-ch-ua");
            //logging.ResponseHeaders.Add("MyResponseHeader");
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        }); ;

        return services;
    }

    public static IServiceCollection AddEbanxSwagger(this IServiceCollection services)
    {
        services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen(c =>
        {
            // Set the comments path for the Swagger JSON and UI.
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml"));
            //search for project documentation files
            var docFiles = Directory.GetFiles(AppContext.BaseDirectory, "Luciano.Serafim.Ebanks.*.xml");
            foreach (var file in docFiles)
            {
                c.IncludeXmlComments(file);
            }
        });

        return services;
    }

    public static IServiceCollection AddEbanxResponse(this IServiceCollection services)
    {
        services.AddScoped(typeof(Response<>), typeof(Response<>));

        return services;
    }

    public static IServiceCollection AddEbanxServices(this IServiceCollection services)
    {
        services.AddSingleton<IAccountService,AccountService>();
        services.AddSingleton<IEventService,EventService>();

        return services;
    }

    public static IServiceCollection AddEbanxAll(this IServiceCollection services)
    {
        services
            .AddEbanxMediatR()
            .AddLogging()
            .AddEbanxHttpLogging()
            .AddEbanxDistributedCache()
            .AddEbanxControllers()
            .AddEbanxSwagger()
            .AddEbanxResponse()
            .AddEbanxServices();

        return services;
    }
}
