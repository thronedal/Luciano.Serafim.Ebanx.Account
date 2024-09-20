using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;
using Luciano.Serafim.Ebanx.Account.Bootstrap.Filters;
using Luciano.Serafim.Ebanx.Account.Bootstrap.MediatR;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Transactions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Infrastructure;
using Luciano.Serafim.Ebanx.Account.Infrastructure.MongoDb;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static IServiceCollection AddEbanxMediatR(this IServiceCollection services, params Assembly[] extraAssemblies)
    {
        Assembly[] assemblies = [Assembly.Load("Luciano.Serafim.Ebanx.Account.Core")];

        if (extraAssemblies.Length > 0)
        {
            assemblies = assemblies.Concat(extraAssemblies).ToArray();
        }

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddOpenBehavior(typeof(AcidBehaviour<,>));
            cfg.AddOpenBehavior(typeof(CachingInvalidationBehaviour<,>));
            cfg.AddOpenBehavior(typeof(CachingBehaviour<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
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

    public static IServiceCollection AddEbanxServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var databaseSettings = configuration.GetSection("MongoDb").Get<MongoDBSettings>();

        if (databaseSettings is null)
        {
            services.AddSingleton<IAccountService, Infrastructure.AccountService>();
            services.AddSingleton<IEventService, Infrastructure.EventService>();
        }
        else
        {
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDb"));
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = new MongoClientSettings()
                {
                    Scheme = ConnectionStringScheme.MongoDB,
                    Server = new MongoServerAddress(databaseSettings!.Host, databaseSettings.Port),
                    RetryWrites = false,
                    RetryReads = false
                };

                return new MongoClient(settings);
            });

            services.AddScoped<IUnitOfWork, Infrastructure.MongoDb.UnitOfWork>();
            services.AddScoped<IAccountService, Infrastructure.MongoDb.AccountService>();
            services.AddScoped<IEventService, Infrastructure.MongoDb.EventService>();

            Mapping.MapEntities();
        }

        return services;
    }

    public static IServiceCollection AddEbanxAll(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddEbanxMediatR()
            .AddLogging()
            .AddEbanxHttpLogging()
            .AddEbanxDistributedCache()
            .AddEbanxControllers()
            .AddEbanxSwagger()
            .AddEbanxResponse()
            .AddEbanxServices(configuration);

        return services;
    }
}
