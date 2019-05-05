using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Parser.Models;

namespace Parser.Services
{
    public class FileReaderService: IFileReaderService
    {
        private ConfigurationModel _defaultConfiguration;
        public ConfigurationModel DefaultConfiguration
        {
            get => _defaultConfiguration ?? (_defaultConfiguration = new ConfigurationModel(true));
        }

        public ConfigurationModel Configurations
        {
            get => GetDefaultConfigurations();
        }

        public ComputerModel TryGetComputerModel()
        {
            if(!Directory.Exists(Configurations.InputDataFolder))
                Directory.CreateDirectory(Configurations.InputDataFolder);

            string[] filePaths = Directory.GetFiles(Configurations.InputDataFolder);
            foreach(var file in filePaths)
            {
                try
                {
                    var dataString = default(string);
                    using(var reader = new StreamReader(file))
                    {
                        dataString = reader.ReadToEnd();
                    }
                    var model = JsonConvert.DeserializeObject<ComputerModel>(dataString);
                    if(model != null)
                        return model;
                    else
                        continue;
                }
                catch(Exception)
                {
                    continue;
                }
            }
            return null;
        }

        public void AddComputerLog(DateTime dateTime, ComputerModel computerModel)
        {
            if(!Directory.Exists(Configurations.OutputDataFolder))
                Directory.CreateDirectory(Configurations.OutputDataFolder);

            var logFullPath = Path.Combine(Configurations.OutputDataFolder, Constants.LogFileName);

            try
            {
                var model = new LogModel()
                {
                    Date = dateTime,
                    ComputerModel = computerModel
                };

                using(var streamWriter = new StreamWriter(logFullPath, File.Exists(logFullPath)))
                {
                    var json = JsonConvert.SerializeObject(model);
                    streamWriter.WriteLine(json);
                }
            }
            catch(Exception)
            {
                //TODO: Create log for exceptions
            }
        }

        private ConfigurationModel GetDefaultConfigurations()
        {
            var configFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Constants.ConfigurationsFileName);
            if(File.Exists(configFullPath))
            {
                try
                {
                    var dataString = default(string);
                    using(var reader = new StreamReader(configFullPath))
                    {
                        dataString = reader.ReadToEnd();
                    }
                    var configs = JsonConvert.DeserializeObject<ConfigurationModel>(dataString);
                    return configs;
                }
                catch(Exception)
                {
                    return DefaultConfiguration;
                }
            }
            else
            {
                try
                {
                    using(var streamWriter = new StreamWriter(configFullPath, false))
                    {
                        var json = JsonConvert.SerializeObject(DefaultConfiguration);
                        streamWriter.WriteLine(json);
                    }
                }
                catch(Exception)
                {
                    //TODO: Create log for exceptions
                }

                return DefaultConfiguration;
            }
        }
    }
}
