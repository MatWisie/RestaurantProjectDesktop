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
                    price = dishObj["price"].Value<double>()

                };

                dishes.Add(dish);
            }

            return dishes;
        }

        public TableGridModel ExtractTableGridDataFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("JSON string cannot be null or empty");
            }

            JObject jsonGridData = JObject.Parse(json);

            if (jsonGridData == null)
            {
                throw new Exception("Failed to parse JSON");
            }

            TableGridModel gridData = new TableGridModel()
            {
                NumberOfRows = jsonGridData["numberOfRows"].Value<int>(),
                NumberOfColumns = jsonGridData["numberOfColumns"].Value<int>(),
            };

            return gridData;
        }

        public List<TableWithIdModel> ExtractTablesFromJson(string json)
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

            List<TableWithIdModel> tables = new List<TableWithIdModel>();

            foreach (JObject tableObj in jsonArray)
            {
                TableWithIdModel table = new TableWithIdModel
                {
                    Id = tableObj["id"].ToString(),
                    IsAvailable = tableObj["isAvailable"].Value<bool>(),
                    NumberOfSeats = tableObj["numberOfSeats"].Value<int>(),
                    GridRow = tableObj["gridRow"].Value<int>(),
                    GridColumn = tableObj["gridColumn"].Value<int>(),

                };

                tables.Add(table);
            }

            return tables;
        }

        public List<OrderWithId> ExtractOrdersFromJson(string json)
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

            List<OrderWithId> orders = new List<OrderWithId>();

            foreach (JObject orderObj in jsonArray)
            {
                OrderWithId order = new OrderWithId
                {
                    Id = orderObj["id"].Value<int>(),
                    Status = (StatusEnum)orderObj["status"].Value<int>(),
                    Price = orderObj["price"].Value<double>(),
                    Dishes = GetDishes(orderObj["dishModels"] as JArray),
                    TableId = orderObj["tableModelId"].Value<int>(),
                    UserId = orderObj["identityUserId"].ToString(),

                };

                orders.Add(order);
            }

            return orders;
        }

        private List<DishWithIdModel> GetDishes(JArray dishes)
        {
            List<DishWithIdModel> dishesList = new List<DishWithIdModel>();

            foreach (JObject dishObj in dishes)
            {
                var dish = new DishWithIdModel
                {
                    id = dishObj["id"].ToString(),
                    name = dishObj["name"].ToString(),
                    description = dishObj["description"].ToString(),
                    availability = dishObj["availability"].Value<bool>(),
                    price = dishObj["price"].Value<double>()
                };

                dishesList.Add(dish);
            }
            return dishesList;
        }

    }
}
