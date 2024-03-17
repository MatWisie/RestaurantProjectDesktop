using RestaurantDesktop.Model;

namespace RestaurantDesktop.Interface
{
    public interface IJsonService
    {
        string ExtractFromJson(string json, string key);
        List<User> ExtractUsersFromJson(string json);
    }
}