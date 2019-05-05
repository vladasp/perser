using Parser.Models;
using System;

namespace Parser.Services
{
    public interface IFileReaderService
    {
        void AddComputerLog(DateTime dateTime, ComputerModel computerModel);
        ComputerModel TryGetComputerModel();
        ConfigurationModel Configurations { get; }
    }
}
