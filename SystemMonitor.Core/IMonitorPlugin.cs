using System.Threading.Channels;
using SystemMonitor.Core.Models;

namespace SystemMonitor.Core;

/// <summary>
/// Interface for the Plugin
/// </summary>
public interface IMonitorPlugin
{
    public string Name { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// Invoke method to run the Plugin functionality
    /// </summary>
    /// <param name="monitoredData">data monitored from the system</param>
    /// <returns></returns>
    public Task InvokeAsync(Channel<MonitorDataDto> monitorDataChannel);

}