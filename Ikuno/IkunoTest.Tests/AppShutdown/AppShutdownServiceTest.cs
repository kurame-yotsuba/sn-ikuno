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
            // IHostApplicationLifetime�̃��b�N�쐬
            var appMock = new Mock<IHostApplicationLifetime>();
            appMock.Setup(x => x.StopApplication());

            // �R���X�g���N�^�ɓ����p�̃C���X�^���X�쐬
            var appLifetime = appMock.Object;
            var logger = new Mock<ILogger<AppShutdownService>>().Object;

            bool exit = false;
            var service = new AppShutdownService(() => exit, appLifetime, logger);
            var task = service.StartAsync(default);

            // exit��true�ɂ��Ă���1�b�ȓ��ɃT�[�r�X���I������B
            // �������AStopApplication���Ă΂��܂łɂ�1000�b����������ۂ��H
            exit = true;
            Task.Delay(1200).Wait();
            task.IsCompletedSuccessfully.IsTrue();
            appMock.Verify(x => x.StopApplication());
        }
    }
}