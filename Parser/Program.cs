using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace Parser
{
    static class Program
    {
        static void Main(string[] args)
        {
            if(Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch(parameter)
                {
                    case Constants.Install:
                        ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case Constants.Uninstall:
                        ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                }
            }
            else
            {
                var servicesToRun = new ServiceBase[] { new Parser() };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
