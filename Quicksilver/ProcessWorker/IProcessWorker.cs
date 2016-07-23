
using ConfigManager;

namespace Quicksilver.ProcessWorker
{
    public interface IProcessWorker
    {
        void Kill();
        void Restart();
        void Start();
    }
}