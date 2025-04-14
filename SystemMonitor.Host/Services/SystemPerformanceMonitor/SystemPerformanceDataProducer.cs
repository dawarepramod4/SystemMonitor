using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemMonitor.Core.Models;

namespace SystemMonitor.Services.SystemPerformanceMonitor
{
    public class SystemPerformanceDataProducer
    {
        private readonly SystemPerformanceMonitorService monitorService;

        public SystemPerformanceDataProducer(SystemPerformanceMonitorService monitorService)
        {
            this.monitorService = monitorService;
        }


        public async Task ProduceSystemMonitoredData(Channel<MonitorDataDto> dataChannel, int intervalMs, CancellationToken token)
        {

            while (!token.IsCancellationRequested)
            {
                var data = await monitorService.GetPerformanceStatsAsync(intervalMs);
                await dataChannel.Writer.WriteAsync(data);
            }

        }
    }
}
