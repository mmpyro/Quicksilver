namespace FileWatcher
{
    public interface IFileObserver
    {
        void Start();
        void Stop();
    }
}