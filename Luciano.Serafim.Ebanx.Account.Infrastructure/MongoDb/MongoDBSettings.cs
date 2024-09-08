using System;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure.MongoDb;

/// <summary>
/// MongoDb Settings
/// </summary>
public class MongoDBSettings
{
    /// <summary>
    /// Host
    /// </summary>
    public string Host { get; set; } = null!;
    
    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// databasename
    /// </summary>
    public string DatabaseName { get; set; } = null!;
}
