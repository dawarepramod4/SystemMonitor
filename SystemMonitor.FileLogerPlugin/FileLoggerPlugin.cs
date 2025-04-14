using System.Reflection.PortableExecutable;
using System.Threading.Channels;
using SystemMonitor.Core;
using SystemMonitor.Core.Models;
using SystemMonitor.FileLogerPlugin.Services;

namespace SystemMonitor.FileLogerPlugin;

/// <summary>
/// File logger Plugin to log the data to file
/// </summary>
public class FileLoggerPlugin : IMonitorPlugin
{
    public string Name { get; set; } = "logtofile";
    public string Description { get; set; } = "Logs the monitor data to a file";

    //logger service to log the data
    private readonly FileLoggerService _fileLoggerService = new();

    public async Task InvokeAsync(Channel<MonitorDataDto> monitorDataChannel)
    {
        //log the data to file
        await foreach (var monitoredData in monitorDataChannel.Reader.ReadAllAsync())
        {
            try
            {
                _fileLoggerService.LogMonitoredDataToFile(monitoredData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending: {ex.Message}");
            }

        }
    }
}