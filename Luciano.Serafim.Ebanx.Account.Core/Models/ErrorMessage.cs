using System;

namespace Luciano.Serafim.Ebanx.Account.Core.Models;

/// <summary>
/// Defines a error message
/// </summary>
public class ErrorMessage
{

    /// <summary>
    /// creates a error message
    /// </summary>
    /// <param name="trackingId"><see cref="TrackingId"/> identifies the error stack on the log</param>
    /// <param name="errorId">Identifies the error Id on the errors catalog</param>
    /// <param name="message"></param>
    public ErrorMessage(Guid trackingId, string errorId, string? message = null)
    {
        TrackingId = trackingId;
        ErrorId = errorId;
        Message = message;
    }

    /// <summary>
    /// <see cref="TrackingId"/> identifies the error stack on the log
    /// </summary>
    public Guid TrackingId { get; }

    /// <summary>
    /// Identifies the error Id on the errors catalog
    /// </summary>
    public string ErrorId { get; }

    /// <summary>
    /// Error message
    /// </summary>
    public string? Message { get; }
}
