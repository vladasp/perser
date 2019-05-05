using ParseLibrary.Services;
using System;
using System.Timers;

namespace ParserConsole
{
    class Program
    {
        private static IFileReaderService _fileReaderService;
        private static Timer _parseTimer;

        static void Main(string[] args)
        {
            _fileReaderService = new FileReaderService();

            SetupLogTimer(_fileReaderService.Configurations.LogUpdateInterval);

            Console.ReadKey();
        }

        private static void SetupLogTimer(int interval)
        {
            _parseTimer = new Timer();
            _parseTimer.Interval = 5 * 1000 * interval;
            _parseTimer.AutoReset = true;
            _parseTimer.Elapsed += ParseTimerElapsed;
            _parseTimer.Start();
        }

        private static void ParseTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _fileReaderService.ProceedComputerLogs();
        }

    }
}
