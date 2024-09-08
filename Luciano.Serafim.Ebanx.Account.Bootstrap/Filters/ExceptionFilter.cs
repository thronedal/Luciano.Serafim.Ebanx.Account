using Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {        
        logger.LogError(context.Exception, context.Exception.Message);
        switch (context.Exception)
        {
            case IBadRequestException:
                context.Result = new BadRequestObjectResult(0);
                break;
            case INotFoundException:
                context.Result = new NotFoundObjectResult(0);
                break;
            case IConflictException:
                context.Result = new ConflictObjectResult(0);
                break;
            case IUnprocessableContentException:
                context.Result = new UnprocessableEntityObjectResult(0);
                break;
            case IInternalServerErrorException:
                context.Result = new ObjectResult(0)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                break;
            default:
                context.Result = new ObjectResult(0)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                break;
        }
    }
}
