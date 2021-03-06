﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwallowNest.Ikuno.AppShutdown.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SwallowNest.Ikuno.AppShutdown
{
    /// <summary>
    /// 条件が満たされたときに、アプリケーションをシャットダウンするサービス。
    /// </summary>
    public class AppShutdownService : BackgroundService
    {
        /// <summary>
        /// アプリケーションのシャットダウンの判断を行うデリゲート
        /// </summary>
        protected readonly ShutdownPredicate _shutdownPredicate;

        protected readonly IHostApplicationLifetime _applicationLifetime;
        protected readonly ILogger<AppShutdownService> _logger;

        private TimeSpan Interval { get; } = TimeSpan.FromSeconds(1);

        public AppShutdownService(
            ShutdownPredicate shutdownPredicate,
            IHostApplicationLifetime applicationLifetime,
            ILogger<AppShutdownService> logger)
        {
            _shutdownPredicate = shutdownPredicate;
            _applicationLifetime = applicationLifetime;
            _logger = logger;
        }

        /// <summary>
        /// <see cref="Interval"/> の時間ごとにシャットダウンを判断するデリゲートを実行し、
        /// true が返ってきたらアプリケーションをシャットダウンする。
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested is false)
            {
                if (_shutdownPredicate())
                {
                    _logger.LogInformation("The shutdown condition is true.");
                    _applicationLifetime.StopApplication();
                    break;
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}