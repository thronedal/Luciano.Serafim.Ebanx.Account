using System;

namespace Luciano.Serafim.Ebanx.Account.Core.Abstractions.Locking;

/// <summary>
/// Interface defining a resource locking in a usecase, 
/// resource locking blocks comcurrent execution of critical paths on code
/// </summary>
public interface IResourceLocking
{
    /// <summary>
    /// comma separeted list of resources to be locked on Usecase execution
    /// </summary>
    public string Resources { get; }

    /// <summary> 
    /// defines a waiting timeout for locking release, define as 0 will cancel execution if theres a lock active
    /// </summary>
    public TimeSpan TimeOut { get; }
}
