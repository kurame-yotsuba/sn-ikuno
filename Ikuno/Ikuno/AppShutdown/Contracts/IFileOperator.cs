namespace SwallowNest.Ikuno.AppShutdown.Contracts
{
    public interface IFileOperator
    {
        string FilePath { get; }
        bool Exists { get; }
        void CreateFile();
        void DeleteFile();
    }
}