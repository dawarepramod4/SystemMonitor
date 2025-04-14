using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMonitor.Services.OperatingSystemDetectionService;

namespace SystemMonitor.Host.Test.Services.OperatingSystemDetection
{
    public class OperatingSystemDetectionServiceTest
    {
        /// <summary>
        /// Note: This test should be run only in windows environment
        /// </summary>
        [Fact]
        public void GetCurrentPlatform_WhenPlatFormIsWindows_ReturnsCorrectPlatForm()
        {

            var currentPlatform =OperatingSystemDetectionService.GetCurrentPlatform();
            Assert.Equal(Models.OperatingSystemType.Windows,currentPlatform);
        }
    }
}
