using ParseLibrary.Services;
using System.ServiceProcess;
using System.Timers;

namespace Parser
{
    public partial class Parser: ServiceBase
    {
        private IFileReaderService _fileReaderService;
        private Timer _parseTimer;
        public Parser()
        {
            InitializeComponent();
            _fileReaderService = new FileReaderService();
        }

        protected override void OnStart(string[] args)
        {
            SetupLogTimer(_fileReaderService.Configurations.LogUpdateInterval);
        }

        protected override void OnStop()
        {
        }

        private void SetupLogTimer(int interval)
        {
            _parseTimer = new Timer();
            _parseTimer.Interval = 5 * 1000 * interval;
            _parseTimer.AutoReset = true;
            _parseTimer.Elapsed += ParseTimerElapsed;
            _parseTimer.Start();
        }

        private void ParseTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _fileReaderService.ProceedComputerLogs();
        }
    }
}
