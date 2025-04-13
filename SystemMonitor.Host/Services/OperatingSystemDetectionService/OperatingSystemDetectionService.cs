using SystemMonitor.Models;

namespace SystemMonitor.Services.OperatingSystemDetectionService;

public class OperatingSystemDetectionService
{
    public static OperatingSystemType GetCurrentPlatform()
    {
        if (OperatingSystem.IsWindows()) return OperatingSystemType.Windows;
        if (OperatingSystem.IsLinux()) return OperatingSystemType.Linux;
        return OperatingSystem.IsMacOS() ? OperatingSystemType.MacOsx : OperatingSystemType.Other;
    }
}