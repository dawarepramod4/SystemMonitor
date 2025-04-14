using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMonitor.Services.CpuUsageMonitor;

namespace SystemMonitor.Host.Test.Services.CpuUsageMonitor
{
    public class CpuUsageMonitorResolverTest
    {
        private readonly ServiceScopeProvider  serviceScopeProvider = new();

        private CpuUsageMonitorResolver GetService()
        {
           var serviceProvider = serviceScopeProvider.ServiceProvider;
           using var scope = serviceProvider.CreateScope();
           return scope.ServiceProvider.GetRequiredService<CpuUsageMonitorResolver>();

        }

        [Fact]
        public void GetCurrentPlatform_WhenPlatFormIsWindows_ReturnPlatFormType()
        {
            //Arrange
            var service = GetService();

            //Act
            var cpuUsageMonitorService = service.ResolveCpuUsageMonitor();

            Assert.IsType(typeof(WindowsCpuUsageMonitor),cpuUsageMonitorService);
        }
    }
}
