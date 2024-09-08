using System;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;

namespace Luciano.Serafim.Ebanx.Account.Core.Exceptions;

/// <summary>
/// Exception raised for errors not expected
/// </summary>
public class UnhandledException : BaseException, IInternalServerErrorException
{
    private const string message = "Something is not working as intended, contact IT";

    /// <inheritdoc/>
    public UnhandledException() : base("500", message)
    {

    }

    /// <inheritdoc/>
    public UnhandledException(Exception innerException) : base("500", message, innerException)
    {

    }

}