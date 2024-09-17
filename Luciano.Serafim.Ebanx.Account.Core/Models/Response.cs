using System;

namespace Luciano.Serafim.Ebanx.Account.Core.Models;


/// <summary>
/// reponse object
/// </summary>
public abstract class Response
{
    /// <summary>
    /// Trace Id
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// List of errors
    /// </summary>
    public List<ErrorMessage> Errors { get; } = new List<ErrorMessage>();

    /// <summary>
    /// Response object
    /// </summary>
    public object? ResponseObject { get; set; }

    /// <summary>
    /// indicates if the call was successful
    /// </summary>
    public bool IsValid { get => Errors.Count == 0; }

    public int Status { get; set; }    
    
    /// <summary>
    /// Get Response payload
    /// </summary>
    public object? GetResponseObject(Type type) => Convert.ChangeType(ResponseObject, type);

    /// <summary>
    /// Sets the value for response Object
    /// </summary>
    /// <param name="value"></param>
    public void SetResponsePayload(object value) => ResponseObject = value; 
}

public class Response<T> : Response
{   
    /// <summary>
    /// Get Response payload
    /// </summary>
    public T? GetResponseObject() => (T?)ResponseObject;
}
