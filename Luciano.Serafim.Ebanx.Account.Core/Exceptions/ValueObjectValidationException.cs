using System;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;

namespace Luciano.Serafim.Ebanx.Account.Core.Exceptions;

/// <summary>
/// Exception raised for error in object validation
/// </summary>
[Serializable]
public class ValueObjectValidationException : BaseException, IBadRequestException
{
    private const string message = "value '{0}' is not valid for the object '{1}'";

    /// <inheritdoc/>
    public ValueObjectValidationException(string exceptionCatalogIdentification, string value, string property) : base(exceptionCatalogIdentification, string.Format(message, value, property))
    {

    }

    /// <inheritdoc/>
    public ValueObjectValidationException(string exceptionCatalogIdentification, string value, string property, Exception innerException) : base(exceptionCatalogIdentification, string.Format(message, value, property), innerException)
    {

    }

}
