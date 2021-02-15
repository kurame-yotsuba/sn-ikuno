using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SwallowNest.Ikuno.AppShutdown;
using SwallowNest.Ikuno.AppShutdown.Contracts;
using System.IO;

namespace SwallowNest.Ikuno.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    string runningFilePath = "app.running";
                    services.AddAppShutdownService(runningFilePath);
                })
                .Build()
                .Run();
        }
    }
}