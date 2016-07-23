using ConfigManager;
using FileWatcher;
using Quicksilver.Exceptions;
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
        private static string currentDir = Directory.GetCurrentDirectory();

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
                using (var processNotifier = new ProcessNotifier(configuration, new FileFilter(logger), logger))
                {
                    quickSilver = new QuickSilver(configuration, processNotifier, new FileNotifierManager(processNotifier), logger);
                    quickSilver.Run();

                    string line = null;
                    Console.Write("> ");
                    while (keepRunning && (line = Console.ReadLine()) != null)
                    {
                        if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            break;
                        Console.Write("> ");
                    }
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
            if(args.Length == 1)
            {
                switch (args[0].ToLower())
                {
                    case "help":
                        Console.WriteLine("Quicksilver\n");
                        Console.WriteLine("init - creates quicksilver.json file at current directory\n");
                        Console.WriteLine("<dir_path> - directory path where quicksilver.json file is located\n");
                        throw new InteruptedException("");
                    case "init":
                        var configuration = new Configuration();
                        Console.WriteLine("Program to execute");
                        configuration.Program = Console.ReadLine();
                        configuration.DirectoryPath = currentDir;
                        Console.WriteLine("Startup file path");
                        configuration.StartUpFilePath = Console.ReadLine();
                        Console.WriteLine("Filters comma separated values");
                        configuration.Filters = Console.ReadLine().Split(',');
                        CreateConfigurationFile(configuration);
                        throw new InteruptedException("");
                    default:
                        if (Directory.Exists(args[0]))
                            return reader.LoadConfiguration(args[0]);
                        else
                            throw new ArgumentException($"Directory: {args[0]} does not exits.");
                }
            }
            return reader.LoadConfiguration();
        }

        private static void CreateConfigurationFile(Configuration configuration)
        {
            IConfigurationWriter writer = new JSONWriter();
            writer.Save(Path.Combine(currentDir, "quicksilver.json"), configuration);
        }
    }
}
