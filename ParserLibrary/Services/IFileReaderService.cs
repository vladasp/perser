using Parser.Models;
using System;
using System.Collections.Generic;

namespace Parser.Services
{
    public interface IFileReaderService
    {
        void ProceedComputerLogs();
        ConfigurationModel Configurations { get; }
    }
}
