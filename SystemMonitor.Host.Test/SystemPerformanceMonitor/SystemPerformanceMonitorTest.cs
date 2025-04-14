using Microsoft.Extensions.DependencyInjection;
using Moq;
using SystemMonitor.Core.Models;
using SystemMonitor.Models;
using SystemMonitor.Services.CpuUsageMonitor;
using SystemMonitor.Services.OperatingSystemDetectionService;
using SystemMonitor.Services.PluginLoaderService;
using SystemMonitor.Services.SystemPerformanceMonitor;

namespace SystemMonitor.Host.Test.SystemPerformanceMonitor;

public class SystemPerformanceMonitorTest
{
    private readonly Mock<ICpuUsageMonitor> _cpuUsageMonitor = new();

    private SystemPerformanceMonitorService initializeCpuUsageMonitor()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<PluginLoaderService>()
            .AddSingleton<OperatingSystemDetectionService>()
            .AddSingleton<CpuUsageMonitorResolver>()
            .AddKeyedSingleton<ICpuUsageMonitor, WindowsCpuUsageMonitor>(OperatingSystemType.Windows)
            .AddKeyedSingleton<ICpuUsageMonitor, LinuxCpuUsageMonitor>(OperatingSystemType.Linux)
            .BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var cpuUsageMonitorResolver = scope.ServiceProvider.GetRequiredService<CpuUsageMonitorResolver>();
        return new SystemPerformanceMonitorService(cpuUsageMonitorResolver);
    }

    [Fact]
    public async Task GetPerformanceStats_WhenIntervalCorrect_ShouldReturnPerformanceStats()
    {
        //Arrange
        int intervalMs = 1000;
        _cpuUsageMonitor.Setup(cpuUsageMonitor => cpuUsageMonitor.GetCpuUsageAsync(intervalMs))
            .ReturnsAsync(GetCorrectCpuUsage());
        SystemPerformanceMonitorService systemPerformanceMonitorService = initializeCpuUsageMonitor();
        //Act
        MonitorDataDto result = await systemPerformanceMonitorService.GetPerformanceStatsAsync(intervalMs);

        //Assert
        Assert.NotNull(result);
        Assert.InRange(result.CpuUsagePercentage, 0, 100);
    }

    static CpuUsageDto GetCorrectCpuUsage()
    {
        return new CpuUsageDto
        {
            UsagePercent = 50
        };
    }
}