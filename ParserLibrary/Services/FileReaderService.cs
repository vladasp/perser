using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        //private ComputerModel TryGetComputerModel()
        //{
        //    if(!Directory.Exists(Configurations.InputDataFolder))
        //        Directory.CreateDirectory(Configurations.InputDataFolder);

        //    string[] filePaths = Directory.GetFiles(Configurations.InputDataFolder);
        //    foreach(var file in filePaths)
        //    {
        //        try
        //        {
        //            var dataString = default(string);
        //            using(var reader = new StreamReader(file))
        //            {
        //                dataString = reader.ReadToEnd();
        //            }
        //            var model = JsonConvert.DeserializeObject<ComputerModel>(dataString);
        //            if(model != null)
        //                return model;
        //            else
        //                continue;
        //        }
        //        catch(Exception)
        //        {
        //            continue;
        //        }
        //    }
        //    return null;
        //}

        //public void AddComputerLog(DateTime dateTime, ComputerModel computerModel)
        //{
        //    if(!Directory.Exists(Configurations.OutputDataFolder))
        //        Directory.CreateDirectory(Configurations.OutputDataFolder);

        //    var resultFullPath = Path.Combine(Configurations.OutputDataFolder, Constants.ResultsFileName);

        //    try
        //    {
        //        var model = new ResultModel()
        //        {
        //            Date = dateTime,
        //            ComputerModel = computerModel
        //        };

        //        using(var streamWriter = new StreamWriter(resultFullPath, File.Exists(resultFullPath)))
        //        {
        //            var json = JsonConvert.SerializeObject(model);
        //            streamWriter.WriteLine(json);
        //        }
        //    }
        //    catch(Exception)
        //    {
        //        //TODO: Create log for exceptions
        //    }
        //}

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

        public void ProceedComputerLogs()
        {
            if(!Directory.Exists(Configurations.InputDataFolder))
                Directory.CreateDirectory(Configurations.InputDataFolder);

            var resultModels = TryGetResultModels();

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
                    {
                        var existingResultModel = resultModels.FirstOrDefault(item => item.ComputerModel.ComputerName == model.ComputerName);
                        if(existingResultModel != null)
                            resultModels.Remove(existingResultModel);

                        if(model.LoadBehavior != 3)
                        {
                            resultModels.Add(new ResultModel()
                            {
                                Date = DateTime.UtcNow,
                                ComputerModel = model
                            });
                        }

                        var proceedFullPath = Path.Combine(Configurations.InputDataFolder, Constants.ProcessedFolder);

                        if(!Directory.Exists(proceedFullPath))
                            Directory.CreateDirectory(proceedFullPath);

                        var fileName = Path.GetFileName(file);
                        var destFile = Path.Combine(proceedFullPath, fileName);
                        File.Move(fileName, destFile);
                    }
                    else
                    {
                        continue;
                    }
                }
                catch(Exception)
                {
                    continue;
                }
            }

            if(!Directory.Exists(Configurations.OutputDataFolder))
                Directory.CreateDirectory(Configurations.OutputDataFolder);

            var resultFullPath = Path.Combine(Configurations.OutputDataFolder, Constants.ResultsFileName);

            try
            {
                using(var streamWriter = new StreamWriter(resultFullPath, false))
                {
                    var json = JsonConvert.SerializeObject(resultModels);
                    streamWriter.WriteLine(json);
                }
            }
            catch(Exception)
            {
                //TODO: Create log for exceptions
            }
        }

        private List<ResultModel> TryGetResultModels()
        {
            var resultFullPath = Path.Combine(Configurations.OutputDataFolder, Constants.ResultsFileName);

            try
            {
                var dataString = default(string);
                using(var reader = new StreamReader(resultFullPath))
                {
                    dataString = reader.ReadToEnd();
                }
                var models = JsonConvert.DeserializeObject<List<ResultModel>>(dataString);

                if(models != null)
                    return models;
                else
                    return new List<ResultModel>();
            }
            catch(Exception)
            {
                return new List<ResultModel>();
            }
        }
    }
}
