using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SwallowNest.Ikuno.AppShutdown;
using System.Threading.Tasks;

namespace SwallowNest.Ikuno.Tests
{
    [TestClass]
    public class AppShutdownServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var appLifetime = new Mock<IHostApplicationLifetime>().Object;
            var logger = new Mock<ILogger<AppShutdownService>>().Object;

            bool exit = false;
            var service = new AppShutdownService(() => exit, appLifetime, logger);
            var task = service.StartAsync(default);
            exit = true;

            Task.Delay(1000).Wait();
            task.IsCompletedSuccessfully.IsTrue();
        }
    }
}
