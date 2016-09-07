namespace Quicksilver.Logger
{
    public interface ILogger
    {
        void Debug(string message);
        void Warn(string message);
        void Info(string message);
        void Error(string message);
        void Log(string message);
    }
}