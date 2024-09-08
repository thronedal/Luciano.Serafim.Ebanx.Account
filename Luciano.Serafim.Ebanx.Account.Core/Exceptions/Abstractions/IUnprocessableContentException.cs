using System;

namespace Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;

    /// <summary>
    /// The 422 (Unprocessable Content) status code indicates that the server understands the content type of the request content (hence a 415 (Unsupported Media Type) status code is inappropriate), and the syntax of the request content is correct, but it was unable to process the contained instructions.
    /// </summary>
    public interface IUnprocessableContentException
    {
    }
