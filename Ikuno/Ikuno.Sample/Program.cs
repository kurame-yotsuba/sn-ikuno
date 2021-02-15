using Microsoft.Extensions.Hosting;
using SwallowNest.Ikuno.AppShutdown;
using System;

namespace SwallowNest.Ikuno.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddAppShutdownService("hoge.txt");
                })
                .Build()
                .Run();
        }
    }
}
