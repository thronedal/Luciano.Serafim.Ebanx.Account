using Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

namespace Luciano.Serafim.Ebanx.Account.Api.Controllers
{
    /// <summary>
    /// Event Data
    /// </summary>
    public class EventDto
    {
        /// <summary>
        /// Balance of the origin account
        /// </summary>
        public AccountBalanceDto? Origin { get; internal set; }

        /// <summary>
        /// Balance of the destination account
        /// </summary>
        public AccountBalanceDto? Destination { get; internal set; }

        /// <summary>
        /// Converts the base type <see cref="WithdrawResponse"/> to <see cref="EventDto"/>.
        /// </summary>
        /// <param name="response"><see cref="WithdrawResponse"/></param>
        public static explicit operator EventDto(WithdrawResponse response)
        {
            return new EventDto()
            {
                Origin = new AccountBalanceDto()
                {
                    Id = response.Origin.Id,
                    Balance = response.Origin.Balance
                }
            };
        }

        /// <summary>
        /// Converts the base type <see cref="DepositResponse"/> to <see cref="EventDto"/>.
        /// </summary>
        /// <param name="response"><see cref="DepositResponse"/></param>
        public static explicit operator EventDto(DepositResponse response)
        {
            return new EventDto()
            {
                Destination = new AccountBalanceDto()
                {
                    Id = response.Destination.Id,
                    Balance = response.Destination.Balance
                }
            };
        }

        /// <summary>
        /// Converts the base type <see cref="TransferResponse"/> to <see cref="EventDto"/>.
        /// </summary>
        /// <param name="response"><see cref="TransferResponse"/></param>
        public static explicit operator EventDto(TransferResponse response)
        {
            return new EventDto()
            {
                Origin = new AccountBalanceDto()
                {
                    Id = response.Origin.Id,
                    Balance = response.Origin.Balance
                },
                Destination = new AccountBalanceDto()
                {
                    Id = response.Destination.Id,
                    Balance = response.Destination.Balance
                }
            };
        }
    }
}
