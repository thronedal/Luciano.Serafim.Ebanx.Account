using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

public class TransferCommand: IRequest<Response<Event>>
{
}