namespace RestaurantDesktop.Interface
{
    public interface IJsonService
    {
        string ExtractFromJson(string json, string key);
    }
}