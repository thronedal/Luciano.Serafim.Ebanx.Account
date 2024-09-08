using System;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;

namespace Luciano.Serafim.Ebanx.Account.Core.Exceptions;

/// <summary>
/// Exception raised for bussiness rules erros
/// </summary>
public class BussinessRuleException : BaseException, IConflictException
{

    /// <inheritdoc/>
    public BussinessRuleException(string message) : base("409", message)
    {

    }

    /// <inheritdoc/>
    public BussinessRuleException(string message, Exception innerException) : base("409", message, innerException)
    {

    }

}