using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;

public class GetBalanceQuery: IRequest<Response<double>>
{
    public int AccountId { get; set; }
}