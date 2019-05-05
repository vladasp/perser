namespace Parser.Models
{
    public class ConfigurationModel
    {
        /// <summary>
        /// Sets interval in minutes
        /// </summary>
        public int LogUpdateInterval { get; set; }
        public string InputDataFolder { get; set; }
        public string OutputDataFolder { get; set; }

        public ConfigurationModel(bool setdefaultValues = false)
        {
            if(setdefaultValues)
            {
                LogUpdateInterval = 1;
                InputDataFolder = "C:\\Parser\\InputFiles";
                OutputDataFolder = "C:\\Parser\\Log";
            }
        }
    }
}
