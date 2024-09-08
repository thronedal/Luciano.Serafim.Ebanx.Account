using Luciano.Serafim.Ebanx.Account.Core.Enums;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.UseCases;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luciano.Serafim.Ebanx.Account.Api.Controllers
{
    /// <summary>
    /// AccountController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<AccountsController> logger;

        /// <inheritdoc/>
        public AccountsController(IMediator mediator, ILogger<AccountsController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        /// <summary>
        /// Resets app state
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("reset")]
        public async Task<ActionResult<string>> ResetState()
        {
            using (logger.BeginScope(this.GetType().Name))
            {
                var result = (await mediator.Send( new ResetAppStateCommand())).GetResponseObject();
                return await Task.FromResult(Ok(result ? "OK" : "FAIL"));
            }
        }

        /// <summary>
        /// Gets the balance of the informed account
        /// </summary>
        /// <param name="accountId">id of the account</param>
        /// <returns></returns>
        [HttpGet]
        [Route("balance")]
        public async Task<ActionResult<double>> GetAccountBalance([FromQuery(Name = "account_id")] int accountId)
        {
            using (logger.BeginScope(this.GetType().Name))
            {
                var result = await mediator.Send(new GetBalanceQuery(accountId));

                return result.GetResponseObject();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route("event")]
        public async Task<ActionResult<EventDto>> RunEvent([FromBody] RunEventDto @event)
        {
            using (logger.BeginScope(this.GetType().Name))
            {
                EventDto result = @event.Type switch
                {
                    EventType.Deposit => (EventDto)(await mediator.Send(new DepositCommand(@event.Destination.GetValueOrDefault(), @event.Amount))).GetResponseObject(),
                    EventType.Withdraw => (EventDto)(await mediator.Send(new WithdrawCommand(@event.Origin.GetValueOrDefault(), @event.Amount))).GetResponseObject(),
                    EventType.Transfer => (EventDto)(await mediator.Send(new TransferCommand(@event.Origin.GetValueOrDefault(), @event.Destination.GetValueOrDefault(), @event.Amount))).GetResponseObject(),
                    _ => throw new NotImplementedException()
                };
                return Created(string.Empty, result);
            }
        }
    }
}
