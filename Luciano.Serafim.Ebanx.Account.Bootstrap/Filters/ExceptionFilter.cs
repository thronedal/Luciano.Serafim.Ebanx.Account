using System;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Response response;
    private readonly ILogger<ExceptionFilter> logger;

    public ExceptionFilter(Response response, ILogger<ExceptionFilter> logger)
    {
        this.response = response;
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {        
        logger.LogError(context.Exception, context.Exception.Message);
        switch (context.Exception)
        {
            case IBadRequestException:
                context.Result = new BadRequestObjectResult(response);
                break;
            case INotFoundException:
                context.Result = new NotFoundObjectResult(response);
                break;
            case IConflictException:
                context.Result = new ConflictObjectResult(response);
                break;
            case IUnprocessableContentException:
                context.Result = new UnprocessableEntityObjectResult(response);
                break;
            case IInternalServerErrorException:
                context.Result = new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                break;
            default:
                var trackId = Guid.Parse(System.Diagnostics.Activity.Current?.RootId ?? Guid.Empty.ToString());
                //TODO: substitute the message for a generic message
                response.Errors.Add( new ErrorMessage(trackId, "-1", $"Unidentified error, contact suport and inform the tracking id:'{trackId}'"));
                context.Result = new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                break;
        }
    }
}
