using System;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;

namespace Luciano.Serafim.Ebanx.Account.Core.Exceptions;

/// <summary>
/// Exception raised for error in object validation
/// </summary>
[Serializable]
public class ObjectNotFoundException : BaseException, INotFoundException
{
    private const string message = "object '{0}' was not found for '{1}'";

    /// <inheritdoc/>
    public ObjectNotFoundException(string exceptionCatalogIdentification, string value, string property) : base(exceptionCatalogIdentification, string.Format(message, value, property))
    {

    }

    /// <inheritdoc/>
    public ObjectNotFoundException(string exceptionCatalogIdentification, string value, string property, Exception innerException) : base(exceptionCatalogIdentification, string.Format(message, value, property), innerException)
    {

    }
}
