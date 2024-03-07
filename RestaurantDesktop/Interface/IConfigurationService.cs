namespace RestaurantDesktop.Interface
{
    public interface IConfigurationService
    {
        bool AddConfiguration(string key, string value);
        string GetConfiguration(string key);
    }
}