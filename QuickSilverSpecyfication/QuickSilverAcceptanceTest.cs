using ConfigReader;
using FileWatcher;
using NUnit.Framework;
using Quicksilver;
using Quicksilver.Logger;
using Quicksilver.ProcessWorker;
using Shouldly;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Quicksilver.Filters;

namespace QuickSilverSpecyfication
{
    [TestFixture]
    public class QuickSilverAcceptanceTest
    {
        private readonly string _configDirectoryPath;
        private readonly Configuration _configuartion;

        public QuickSilverAcceptanceTest()
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            _configDirectoryPath = Path.GetDirectoryName(ass.Location);
            FileObserver.CreateFunction = (dto, notifier) => new FileWatchDog(dto, notifier);
            _configuartion = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Path.Combine(_configDirectoryPath, "quicksilver.js")));
        }

        [SetUp]
        public void Before()
        {
            if (!Directory.Exists(_configuartion.DirectoryPath))
            {
                Directory.CreateDirectory(_configuartion.DirectoryPath);
                File.Create(Path.Combine(_configuartion.DirectoryPath, _configuartion.StartUpFilePath)).Close();
            }
        }

        [TearDown]
        public void After()
        {
            if (Directory.Exists(_configuartion.DirectoryPath))
            {
                Directory.Delete(_configuartion.DirectoryPath, true);
            }
        }

        [Test]
        public void ShouldStartAndKillQuickSilver()
        {
            //Given
            ILogger logger = new ConsoleLogger();
            IConfigurationReader reader = new JSONReader();
            Configuration configuration = reader.LoadConfiguration(_configDirectoryPath);
            IProcessNotifier processNotifier = new ProcessNotifier( configuration, new FileFilter(logger), logger);
            QuickSilver quickSilver = new QuickSilver(configuration, processNotifier, new FileNotifierManager(processNotifier), logger);
            //When
            quickSilver.Run();
            File.Create(Path.Combine(_configuartion.DirectoryPath, "aa.js")).Close();
            quickSilver.Kill();
        }
    }
}
