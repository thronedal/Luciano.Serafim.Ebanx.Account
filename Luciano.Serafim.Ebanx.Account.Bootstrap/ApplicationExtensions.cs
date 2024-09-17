using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap;

[ExcludeFromCodeCoverage]
public static class ApplicationExtensions
{
    public static WebApplication UseEbanxApi(this WebApplication app)
    {
        app.UseHttpLogging();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;

    }
}
