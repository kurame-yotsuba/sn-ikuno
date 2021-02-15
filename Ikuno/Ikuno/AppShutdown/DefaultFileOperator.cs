using SwallowNest.Ikuno.AppShutdown.Contracts;
using System.IO;

namespace SwallowNest.Ikuno.AppShutdown
{
    internal class DefaultFileOperator : IFileOperator
    {
        private readonly string _filePath;

        public DefaultFileOperator(string filePath) => _filePath = filePath;

        public string FilePath => _filePath;

        public bool Exists => File.Exists(FilePath);

        public void CreateFile() => File.Create(FilePath).Close();

        public void DeleteFile() => File.Delete(FilePath);
    }
}