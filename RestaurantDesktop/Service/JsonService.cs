using Newtonsoft.Json.Linq;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;

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
        public List<User> ExtractUsersFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("JSON string cannot be null or empty");
            }

            JArray jsonArray = JArray.Parse(json);

            if (jsonArray == null)
            {
                throw new Exception("Failed to parse JSON");
            }

            List<User> users = new List<User>();

            foreach (JObject userObj in jsonArray)
            {
                User user = new User
                {
                    Id = userObj["id"].ToString(),
                    UserName = userObj["userName"].ToString(),
                    Role = userObj["role"].ToString()
                };

                users.Add(user);
            }

            return users;
        }
        public List<DishWithIdModel> ExtractDishesFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("JSON string cannot be null or empty");
            }

            JArray jsonArray = JArray.Parse(json);

            if (jsonArray == null)
            {
                throw new Exception("Failed to parse JSON");
            }

            List<DishWithIdModel> dishes = new List<DishWithIdModel>();

            foreach (JObject dishObj in jsonArray)
            {
                DishWithIdModel dish = new DishWithIdModel
                {
                    id = dishObj["id"].ToString(),
                    name = dishObj["name"].ToString(),
                    description = dishObj["description"].ToString(),
                    availability = dishObj["availability"].Value<bool>(),
                    price = dishObj["availability"].Value<double>()

                };

                dishes.Add(dish);
            }

            return dishes;
        }
    }
}
