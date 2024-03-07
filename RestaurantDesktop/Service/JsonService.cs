using Newtonsoft.Json.Linq;
using RestaurantDesktop.Interface;

namespace RestaurantDesktop.Service
{
    public class JsonService : IJsonService
    {
        public string ExtractFromJson(string json, string key)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("Key can't be null in ExtractFromJson");
            }

            JObject jsonResponse = JObject.Parse(json);
            if (jsonResponse == null)
                throw new Exception(jsonResponse + " was null");

            string token = (string)jsonResponse[key];

            if (token == null)
                throw new Exception(key + " does not exist in response");

            return token;
        }
    }
}
