using ConfigReader;
using FileWatcher;
using Quicksilver.Filters;
using Quicksilver.Logger;
using Quicksilver.ProcessWorker;
using System;
using System.IO;

namespace Quicksilver
{
    public class Program
    {
        private static QuickSilver quickSilver;
        private static bool keepRunning = true;
        public static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                keepRunning = false;
            };

            FileObserver.CreateFunction = (dto, notifier) => new FileWatchDog(dto, notifier);
            ILogger logger = null;
            try
            {
                logger = new ConsoleLogger();
                IConfigurationReader reader = new JSONReader();
                Configuration configuration = LoadConfiguration(reader, args);
                IProcessNotifier processNotifier = new ProcessNotifier(configuration, new FileFilter(logger), logger);
                quickSilver = new QuickSilver(configuration, processNotifier, new FileNotifierManager(processNotifier), logger);
                quickSilver.Run();
                string line = null;
                Console.Write("> ");
                while( keepRunning && (line = Console.ReadLine()) != null )
                {
                    if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        break;
                    Console.Write("> ");
                }
            }
            catch(Exception ex)
            {
                logger?.Error(ex.Message);
            }
            finally
            {
                quickSilver?.Kill();
            }
        }

        private static Configuration LoadConfiguration(IConfigurationReader reader, string[] args)
        {
            if(args.Length == 1 && Directory.Exists(args[0]))
            {
                return reader.LoadConfiguration(args[0]);
            }
            return reader.LoadConfiguration();
        }
    }
}
