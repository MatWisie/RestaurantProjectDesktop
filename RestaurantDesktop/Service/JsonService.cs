using Newtonsoft.Json.Linq;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;

namespace RestaurantDesktop.Service
{
    public class JsonService : IJsonService
    {
        public string ExtractFromJson(string json, string key)
        {
            CheckJsonFormat(json, key);

            JObject jsonResponse = JObject.Parse(json);

            CheckJsonNull(jsonResponse);

            JToken token = jsonResponse.SelectToken(key);

            CheckJTokenNull(token);

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
            CheckJsonFormat(json);

            JArray jsonArray = JArray.Parse(json);

            CheckJsonNull(jsonArray);

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
            CheckJsonFormat(json);

            JArray jsonArray = JArray.Parse(json);

            CheckJsonNull(jsonArray);

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
            CheckJsonFormat(json);

            JObject jsonGridData = JObject.Parse(json);

            CheckJsonNull(jsonGridData);

            TableGridModel gridData = new TableGridModel()
            {
                NumberOfRows = jsonGridData["numberOfRows"].Value<int>(),
                NumberOfColumns = jsonGridData["numberOfColumns"].Value<int>(),
            };

            return gridData;
        }

        public List<TableWithIdModel> ExtractTablesFromJson(string json)
        {
            CheckJsonFormat(json);

            JArray jsonArray = JArray.Parse(json);

            CheckJsonNull(json);

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
            CheckJsonFormat(json);

            JArray jsonArray = JArray.Parse(json);

            CheckJsonNull(jsonArray);

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

        public List<ReservationModel> ExtractReservationsFromJson(string json)
        {
            CheckJsonFormat(json);

            JArray jsonArray = JArray.Parse(json);

            CheckJsonNull(json);

            List<ReservationModel> reservations = new List<ReservationModel>();

            foreach (JObject tableObj in jsonArray)
            {
                ReservationModel reservation = new ReservationModel
                {
                    Id = tableObj["id"].ToString(),
                    From = tableObj["from"].Value<DateTime>(),
                    To = tableObj["to"].Value<DateTime>(),
                    UserId = tableObj["identityUserId"].ToString(),
                    TableId = tableObj["tableModelId"].Value<int>(),
                };

                reservations.Add(reservation);
            }

            return reservations;
        }

        public List<LoginLogModel> ExtractLoginLogsFromJson(string json)
        {
            CheckJsonFormat(json);

            JArray jsonArray = JArray.Parse(json);

            CheckJsonNull(jsonArray);

            List<LoginLogModel> logs = new List<LoginLogModel>();

            foreach (JObject logObj in jsonArray)
            {
                LoginLogModel log = new LoginLogModel
                {
                    Id = logObj["id"].Value<int>(),
                    IpAddress = logObj["ipAddress"].ToString(),
                    Success = logObj["success"].Value<bool>(),
                    CreatedAt = logObj["createdAt"].Value<DateTime>(),
                    StatusCode = logObj["statusCode"].ToString(),
                    IdentityUserId = logObj["identityUserId"].ToString(),
                };

                logs.Add(log);
            }

            return logs;
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

        private bool CheckJsonFormat(string json, string key)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("Key or JSON string cannot be null or empty");
            }

            return true;
        }

        private bool CheckJsonFormat(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("Key or JSON string cannot be null or empty");
            }

            return true;
        }

        private bool CheckJsonNull(object jsonResponse)
        {
            if (jsonResponse == null)
            {
                throw new Exception("Failed to parse JSON");
            }
            return true;
        }

        private bool CheckJTokenNull(JToken token)
        {
            if (token == null)
            {
                throw new Exception("Key does not exist in response");
            }
            return true;
        }

    }
}
