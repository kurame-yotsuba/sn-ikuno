﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwallowNest.Ikuno.AppShutdown.Contracts;
using System;

namespace SwallowNest.Ikuno.AppShutdown
{
    public static class AppShutdownExtensions
    {
        private static (IHostApplicationLifetime, ILogger<AppShutdownService>) Get(IServiceProvider provider)
        {
            var appLifetime = provider.GetService<IHostApplicationLifetime>();
            var logger = provider.GetService<ILogger<AppShutdownService>>();

            if (appLifetime is null)
            {
                throw new InvalidOperationException($"{nameof(IHostApplicationLifetime)} instance is not registered in services.");
            }

            if (logger is null)
            {
                throw new InvalidOperationException($"{nameof(ILogger<AppShutdownService>)} instance is not registered in services.");
            }

            return (appLifetime, logger);
        }

        /// <summary>
        /// 条件が満たされたときに、アプリケーションのシャットダウンを行うサービスを追加する。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="shutdownDiscriminator">シャットダウンの判断を行うデリゲート</param>
        /// <returns></returns>
        public static IServiceCollection AddAppShutdownService(
            this IServiceCollection services,
            ShutdownPredicate shutdownPredicate)
        {
            services.AddHostedService(provider =>
            {
                var (appLifetime, logger) = Get(provider);
                return new AppShutdownService(shutdownPredicate, appLifetime, logger);
            });

            return services;
        }

        /// <summary>
        /// 指定されたファイルが存在しなくなったときに、アプリケーションのシャットダウンを行うサービスを追加する。
        /// シャットダウン時に指定されたファイルが残っていた場合はそのファイルは削除される。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="fileOperator">ファイルアクセス用のインスタンス</param>
        /// <returns></returns>
        public static IServiceCollection AddAppShutdownService(
            this IServiceCollection services,
            IFileOperator fileOperator)
        {
            services.AddHostedService(provider =>
            {
                var (appLifetime, logger) = Get(provider);
                return new FileExistanceShutdownService(fileOperator, appLifetime, logger);
            });

            return services;
        }

        /// <summary>
        /// 指定されたファイルが存在しなくなったときに、アプリケーションのシャットダウンを行うサービスを追加する。
        /// シャットダウン時に指定されたファイルが残っていた場合はそのファイルは削除される。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="filePath">アプリケーションが実行中であることを表すファイルのパス</param>
        /// <returns></returns>
        public static IServiceCollection AddAppShutdownService(
            this IServiceCollection services,
            string filePath)
        {
            IFileOperator fileOperator = new DefaultFileOperator(filePath);
            return AddAppShutdownService(services, fileOperator);
        }
    }
}