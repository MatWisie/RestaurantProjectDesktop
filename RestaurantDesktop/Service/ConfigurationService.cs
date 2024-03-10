using RestaurantDesktop.Interface;
using System.Configuration;

namespace RestaurantDesktop.Service
{
    public class ConfigurationService : IConfigurationService
    {
        public bool AddConfiguration(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (config.AppSettings.Settings[key] == null)
                    config.AppSettings.Settings.Add(key, value);
                else
                    config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string GetConfiguration(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config.AppSettings.Settings[key].Value;
        }
    }
}
