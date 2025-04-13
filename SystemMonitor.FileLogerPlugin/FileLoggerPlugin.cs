using SystemMonitor.Core;
using SystemMonitor.Core.Models;
using SystemMonitor.FileLogerPlugin.Services;

namespace SystemMonitor.FileLogerPlugin;

/// <summary>
/// File logger Plugin to log the data to file
/// </summary>
public class FileLoggerPlugin : IMonitorPlugin
{
    public string Name { get; set; } = "SystemMonitor.FileLogerPlugin";
    public string Description { get; set; } = "Logs the monitor data to a file";
    
    //logger service to log the data
    private readonly FileLoggerService _fileLoggerService= new();
    
    public Task InvokeAsync(MonitorDataDto monitoredData)
    {
        //log the data to file
        _fileLoggerService.LogMonitoredDataToFile(monitoredData);
        return Task.CompletedTask;
    }
}