using ParseLibrary.Models;

namespace ParseLibrary.Services
{
    public interface IFileReaderService
    {
        void ProceedComputerLogs();
        ConfigurationModel Configurations { get; }
    }
}
