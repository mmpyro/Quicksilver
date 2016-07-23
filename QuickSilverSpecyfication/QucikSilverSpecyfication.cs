using NUnit.Framework;
using Quicksilver;
using NSubstitute;
using Ploeh.AutoFixture;
using ConfigReader;
using FileWatcher;
using FileNotifier;
using System.IO;
using Quicksilver.Logger;
using Quicksilver.ProcessWorker;

namespace QuickSilverSpecyfication
{
    [TestFixture]
    public class QucikSilverSpecyfication
    {
        private readonly Fixture _any = new Fixture();

        private IProcessNotifier CreateProcess()
        {
            var process = Substitute.For<IProcessNotifier>();
            process.When(t => t.OnCreated(Arg.Any<FileSystemEventArgs>())).Do(t => process.Restart());
            return process;
        }

        private IConfigurationReader CreateConfigReader(Configuration configuration)
        {
            var configReader = Substitute.For<IConfigurationReader>();
            configReader.LoadConfiguration(Arg.Any<string>()).Returns(x => configuration);
            return configReader;
        }

        private ILogger CreateLogger()
        {
            ILogger logger = Substitute.For<ILogger>();
            logger.Debug(Arg.Any<string>());
            return logger;
        }

        public QucikSilverSpecyfication()
        {
            FileObserver.CreateFunction = (dto, notifier) => CreateFileWatchDog(dto, notifier);
        }

        [Test, Category("Unit")]
        public void ShouldStartNewProcess()
        {
            //Given
            Configuration configuration = _any.Create<Configuration>();
            IProcessNotifier processWorker = CreateProcess();
            ILogger logger = CreateLogger();
            FileNotifierManager fileManager = new FileNotifierManager(processWorker);
            QuickSilver quicksilver = new QuickSilver(configuration, processWorker, fileManager, logger);
            //When
            quicksilver.Run();
            //Then
            processWorker.Received(1).Start();
            logger.Received(1).Info("Quicksiler started.");
            logger.Received(1).Info("Configuration was loaded.");
        }

        [Test, Category("Unit")]
        public void ShouldKillProcess()
        {
            //Given
            Configuration configuration = _any.Create<Configuration>();
            IProcessNotifier processWorker = CreateProcess();
            ILogger logger = CreateLogger();
            FileNotifierManager fileManager = new FileNotifierManager(processWorker);
            QuickSilver quicksilver = new QuickSilver(configuration, processWorker, fileManager, logger);
            //When
            quicksilver.Run();
            quicksilver.Kill();
            //Then
            processWorker.Received(1).Kill();
            logger.Received(1).Warn("Quicksiler will be closed");
        }

        [Test, Category("Unit")]
        public void ShouldRestartProcess()
        {
            //Given
            Configuration configuration = _any.Create<Configuration>();
            IProcessNotifier processWorker = CreateProcess();
            ILogger logger = CreateLogger();
            FileNotifierManager fileManager = new FileNotifierManager(processWorker);
            QuickSilver quicksilver = new QuickSilver(configuration, processWorker, fileManager, logger);
            //When
            quicksilver.Run();
            quicksilver.Restart();
            //Then
            processWorker.Received(1).Start();
            processWorker.Received(1).Restart();
        }

        [Test, Category("Unit")]
        public void ShouldRestartProcessWhenEventRaise()
        {
            //Given
            Configuration configuration = _any.Create<Configuration>();
            IProcessNotifier processWorker = CreateProcess();
            ILogger logger = CreateLogger();
            FileNotifierManager fileManager = new FileNotifierManager(processWorker);
            QuickSilver quicksilver = new QuickSilver(configuration, processWorker, fileManager, logger);
            //When
            quicksilver.Run();
            processWorker.OnCreated(_any.Create<FileSystemEventArgs>());
            //Then
            processWorker.Received(1).Restart();
        }
        
        private IFileObserver CreateFileWatchDog(ObserveFileDto dto, IFileNotifier[] notifier)
        {
            return new FakeWatchDog(dto, notifier);
        }
    }
}
