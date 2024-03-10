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
                throw new ArgumentNullException("Key or JSON string cannot be null or empty");
            }

            JObject jsonResponse = JObject.Parse(json);

            if (jsonResponse == null)
            {
                throw new Exception("Failed to parse JSON");
            }

            JToken token = jsonResponse.SelectToken(key);

            if (token == null)
            {
                throw new Exception("Key does not exist in response");
            }

            if (token.Type == JTokenType.Array)
            {
                JArray array = (JArray)token;
                if (array.Count > 0)
                {
                    return array[0].ToString();
                }
                else
                {
                    throw new Exception("Array is empty");
                }
            }
            else
            {
                return token.ToString();
            }
        }
    }
}
