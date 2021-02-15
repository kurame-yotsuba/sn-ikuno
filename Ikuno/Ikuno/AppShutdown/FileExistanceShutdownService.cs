using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwallowNest.Ikuno.AppShutdown.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace SwallowNest.Ikuno.AppShutdown
{
    /// <summary>
    /// ファイルの存在可否によってアプリケーションをシャットダウンするサービス。
    /// ファイルがなくなったら、アプリケーションをシャットダウンする。
    /// </summary>
    public class FileExistanceShutdownService : AppShutdownService
    {
        private readonly IFileOperator _fileOperator;

        public FileExistanceShutdownService(
            IFileOperator fileOperator,
            IHostApplicationLifetime applicationLifetime,
            ILogger<AppShutdownService> logger)
            : base(() => !fileOperator.Exists, applicationLifetime, logger)
        {
            _fileOperator = fileOperator;
        }

        /// <summary>
        /// ファイルの作成を行う。
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (_fileOperator.Exists is false)
            {
                _fileOperator.CreateFile();
                _logger.LogInformation("The app running file is created.");
            }
            else
            {
                string message = "{0} has already existed. It will be deleted when the app is shutdowned.";
                _logger.LogWarning(message, _fileOperator.FilePath);
            }

            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// ファイルの削除を行う。
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            if (_fileOperator.Exists)
            {
                _fileOperator.DeleteFile();
                _logger.LogInformation("The app running file is deleted.");
            }

            return base.StopAsync(cancellationToken);
        }
    }
}