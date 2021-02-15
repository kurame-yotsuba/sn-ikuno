namespace SwallowNest.Ikuno.AppShutdown
{
    public interface IFileOperator
    {
        string FilePath { get; }
        bool Exists { get; }
        void CreateFile();
        void DeleteFile();
    }
}