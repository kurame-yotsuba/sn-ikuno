using CoreTweet;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwallowNest.Ikuno.TweetBot.HomeTimeline.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwallowNest.Ikuno.TweetBot
{
    public class HomeTimelineService : BackgroundService
    {
        readonly ILogger<HomeTimelineService> _logger;
        readonly TimeSpan _interval;

        public HomeTimelineService(ILogger<HomeTimelineService> logger)
        {
            _logger = logger;
            _interval = TimeSpan.FromSeconds(3);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(stoppingToken.IsCancellationRequested is false)
            {
                _logger.LogInformation("タイムライン取得");
                _tokens.Statuses.HomeTimeline()
                await Task.Delay(_interval);
            }
        }
    }
}
