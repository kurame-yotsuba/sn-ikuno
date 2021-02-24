using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SwallowNest.Ikuno.AppShutdown;
using System.Threading.Tasks;

namespace SwallowNest.Ikuno.Tests.AppShutdown
{
    [TestClass]
    public class AppShutdownServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // IHostApplicationLifetimeのモック作成
            var appMock = new Mock<IHostApplicationLifetime>();
            appMock.Setup(x => x.StopApplication());

            // コンストラクタに入れる用のインスタンス作成
            var appLifetime = appMock.Object;
            var logger = new Mock<ILogger<AppShutdownService>>().Object;

            bool exit = false;
            var service = new AppShutdownService(() => exit, appLifetime, logger);
            var task = service.StartAsync(default);

            // exitをtrueにしてから1秒以内にサービスが終了する。
            // ただし、StopApplicationが呼ばれるまでには1000秒強かかるっぽい？
            exit = true;
            Task.Delay(1200).Wait();
            task.IsCompletedSuccessfully.IsTrue();
            appMock.Verify(x => x.StopApplication());
        }
    }
}