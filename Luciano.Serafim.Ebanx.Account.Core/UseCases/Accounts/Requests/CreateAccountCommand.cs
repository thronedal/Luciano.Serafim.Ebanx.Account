using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;

public class CreateAccountCommand : IRequest<Response<Models.Account>>
{
    public CreateAccountCommand(int accountId)
    {
        AccountId = accountId;
    }

    /// <summary>
    /// Account id to be created
    /// </summary>
    public int AccountId { get; internal set; }

    /// <summary>
    /// Converts the base type <see cref="CreateAccountCommand"/> to <see cref="Event"/>.
    /// </summary>
    /// <param name="command"><see cref="CreateAccountCommand"/></param>
    public static explicit operator Models.Account(CreateAccountCommand command)
    {
        return new Models.Account(command.AccountId, command.AccountId.ToString());
    }
}
