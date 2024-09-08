using System;
using System.Diagnostics.CodeAnalysis;
using Luciano.Serafim.Ebanx.Account.Core.Models;

namespace Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;

/// <summary>
/// Base type for exceptions, application exceptions should inherit it
/// </summary>
[ExcludeFromCodeCoverage]
[Serializable]
public abstract class BaseException : Exception
{
    /// <summary>
    /// identifies the exception on the error catalog
    /// </summary>
    public string ExceptionCatalogIdentification { get; set; }

    /// <summary>
    /// identifies the tracer generated
    /// </summary>
    public Guid TraceID => Guid.Parse(System.Diagnostics.Activity.Current?.RootId ?? Guid.Empty.ToString());

    /// <summary>
    /// instatiates a exception
    /// </summary>
    protected BaseException(string exceptionCatalogIdentification) : base()
    {
        ExceptionCatalogIdentification = exceptionCatalogIdentification;
    }


    /// <summary>
    /// instatiates a exception
    /// </summary>
    protected BaseException(string exceptionCatalogIdentification, string message) : base(message)
    {
        ExceptionCatalogIdentification = exceptionCatalogIdentification;
    }


    /// <summary>
    /// instatiates a exception
    /// </summary>
    protected BaseException(string exceptionCatalogIdentification, string message, Exception innerException) : base(message, innerException)
    {
        ExceptionCatalogIdentification = exceptionCatalogIdentification;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    public static implicit operator ErrorMessage(BaseException exception)
    {
        return new ErrorMessage(exception.TraceID, exception.ExceptionCatalogIdentification, exception.Message);
    }
}
