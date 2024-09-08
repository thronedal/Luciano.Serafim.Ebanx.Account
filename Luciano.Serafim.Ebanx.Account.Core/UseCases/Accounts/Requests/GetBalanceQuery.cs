using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;

public class GetBalanceQuery: IRequest<Response<double>>
{

    public GetBalanceQuery(int accountId)
    {
        AccountId = accountId;
    }

    /// <summary>
    /// Account Id
    /// </summary>
    public int AccountId { get; internal set; }
}