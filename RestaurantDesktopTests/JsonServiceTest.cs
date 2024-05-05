using Newtonsoft.Json;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;

namespace RestaurantDesktopTests
{
    [TestClass]
    public class JsonServiceTest
    {
        [TestMethod]
        public void ExtractOrdersFromJson_CorrectOneObjJson_CorrectValuesWithoutDish()
        {
            JsonService service = new JsonService();
            string json = "[\r\n    {\r\n        \"id\": 1,\r\n        \"status\": 2,\r\n        \"price\": 2,\r\n        \"tableModelId\": 6,\r\n        \"tableModel\": {\r\n            \"id\": 6,\r\n            \"isAvailable\": true,\r\n            \"numberOfSeats\": 2,\r\n            \"gridRow\": 0,\r\n            \"gridColumn\": 0,\r\n            \"orderModels\": null,\r\n            \"reservations\": null\r\n        },\r\n        \"dishModels\": [\r\n            {\r\n                \"id\": 3,\r\n                \"name\": \"LongDescDish\",\r\n                \"description\": \"jmnawldnalsndalndwlansdlkanwlkdnsalinwdlkasndlwikandsm.,andalikdnalskdnakl\",\r\n                \"price\": 4,\r\n                \"availability\": true,\r\n                \"orders\": null\r\n            }\r\n        ],\r\n        \"identityUserId\": \"774f7b87-d9ff-4efb-8c4c-1f1112efdf3c\",\r\n        \"identityUserModel\": null\r\n    }\r\n]";
            var result = service.ExtractOrdersFromJson(json);

            List<OrderWithId> expectedResult = new List<OrderWithId>()
            {
                new OrderWithId()
                {
                    Id = 1,
                    Status = StatusEnum.Working,
                    Price = 2,
                    TableId = 6,
                }
            };

            Assert.AreEqual(expectedResult[0].Id, result[0].Id);
            Assert.AreEqual(expectedResult[0].Status, result[0].Status);
            Assert.AreEqual(expectedResult[0].Price, result[0].Price);
            Assert.AreEqual(expectedResult[0].TableId, result[0].TableId);
        }

        [TestMethod]
        public void ExtractOrdersFromJson_CorrectOneObjJson_CorrectDishesInOrder()
        {
            JsonService service = new JsonService();
            string json = "[\r\n    {\r\n        \"id\": 1,\r\n        \"status\": 2,\r\n        \"price\": 2,\r\n        \"tableModelId\": 6,\r\n        \"tableModel\": {\r\n            \"id\": 6,\r\n            \"isAvailable\": true,\r\n            \"numberOfSeats\": 2,\r\n            \"gridRow\": 0,\r\n            \"gridColumn\": 0,\r\n            \"orderModels\": null,\r\n            \"reservations\": null\r\n        },\r\n        \"dishModels\": [\r\n            {\r\n                \"id\": 3,\r\n                \"name\": \"LongDescDish\",\r\n                \"description\": \"jmnawldnalsndalndwlansdlkanwlkdnsalinwdlkasndlwikandsm.,andalikdnalskdnakl\",\r\n                \"price\": 4,\r\n                \"availability\": true,\r\n                \"orders\": null\r\n            }\r\n        ],\r\n        \"identityUserId\": \"774f7b87-d9ff-4efb-8c4c-1f1112efdf3c\",\r\n        \"identityUserModel\": null\r\n    }\r\n]";
            List<OrderWithId> expectedResult = new List<OrderWithId>()
            {
                new OrderWithId()
                {
                    Id = 1,
                    Status = StatusEnum.Working,
                    Price = 2,
                    TableId = 6,
                    Dishes = new List<DishWithIdModel>()
                    {
                        new DishWithIdModel()
                        {
                            id = "3",
                            name = "LongDescDish",
                            description = "jmnawldnalsndalndwlansdlkanwlkdnsalinwdlkasndlwikandsm.,andalikdnalskdnakl",
                            price = 4
                        }
                    }
                }
            };
            var result = service.ExtractOrdersFromJson(json);


            Assert.AreEqual(expectedResult[0].Dishes[0].id, result[0].Dishes[0].id);
            Assert.AreEqual(expectedResult[0].Dishes[0].name, result[0].Dishes[0].name);
            Assert.AreEqual(expectedResult[0].Dishes[0].description, result[0].Dishes[0].description);
            Assert.AreEqual(expectedResult[0].Dishes[0].price, result[0].Dishes[0].price);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void ExtractOrdersFromJson_InCorrectJson_JsonReaderException()
        {
            JsonService service = new JsonService();
            string json = "bla";
            var result = service.ExtractOrdersFromJson(json);

        }

        [TestMethod]
        public void ExtractReservationsFromJson_CorrectOneObjJson_CorrectReservation()
        {
            JsonService service = new JsonService();
            string json = "[\r\n  {\r\n    \"id\": 2,\r\n    \"from\": \"2024-04-17T16:24:29.134Z\",\r\n    \"to\": \"2024-04-17T17:24:29.134Z\",\r\n    \"identityUserId\": \"user\",\r\n    \"tableModelId\": 2\r\n  }\r\n]";

            List<ReservationModel> expectedResult = new List<ReservationModel>()
            {
                new ReservationModel()
                {
                    Id = "2",
                    From = new DateTime(2024,4,17,16,24,29,134),
                    To = new DateTime(2024,4,17,17,24,29,134),
                    UserId = "user",
                    TableId = 2
                }
            };

            var result = service.ExtractReservationsFromJson(json);

            Assert.AreEqual(expectedResult[0].Id, result[0].Id);
            Assert.AreEqual(expectedResult[0].From, result[0].From);
            Assert.AreEqual(expectedResult[0].To, result[0].To);
            Assert.AreEqual(expectedResult[0].UserId, result[0].UserId);
            Assert.AreEqual(expectedResult[0].TableId, result[0].TableId);
        }

        [TestMethod]
        public void ExtractLoginLogsFromJson_CorrectOneObjJson_CorrectLogValues()
        {
            JsonService service = new JsonService();
            string json = "[\r\n    {\r\n        \"id\": 1,\r\n        \"ipAddress\": \"91.225.159.196\",\r\n        \"success\": true,\r\n        \"createdAt\": \"2024-04-28T18:50:18.8853813\",\r\n        \"statusCode\": 200,\r\n        \"identityUserId\": \"774f7b87-d9ff-4efb-8c4c-1f1112efdf3c\",\r\n        \"identityUserModel\": null\r\n    }\r\n]";

            List<LoginLogModel> expectedResult = new List<LoginLogModel>()
            {
                new LoginLogModel()
                {
                    Id = 1,
                    IpAddress = "91.225.159.196",
                    Success = true,
                    CreatedAt = new DateTime(2024, 4, 28, 18, 50, 18),
                    StatusCode = "200",
                    IdentityUserId = "774f7b87-d9ff-4efb-8c4c-1f1112efdf3c"
                }
            };

            var result = service.ExtractLoginLogsFromJson(json);
            var dateWithoutMiliSeconds = new DateTime(result[0].CreatedAt.Year, result[0].CreatedAt.Month, result[0].CreatedAt.Day, result[0].CreatedAt.Hour, result[0].CreatedAt.Minute, result[0].CreatedAt.Second);

            Assert.AreEqual(expectedResult[0].Id, result[0].Id);
            Assert.AreEqual(expectedResult[0].IpAddress, result[0].IpAddress);
            Assert.AreEqual(expectedResult[0].Success, result[0].Success);
            Assert.AreEqual(expectedResult[0].CreatedAt, dateWithoutMiliSeconds);
            Assert.AreEqual(expectedResult[0].StatusCode, result[0].StatusCode);
            Assert.AreEqual(expectedResult[0].IdentityUserId, result[0].IdentityUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void ExtractLoginLogsFromJson_InCorrectJson_JsonReaderException()
        {
            JsonService service = new JsonService();
            string json = "bla";
            var result = service.ExtractOrdersFromJson(json);

        }

        [TestMethod]
        public void ExtractUsersFromJson_CorrectOneObjJson_CorrectUser()
        {
            JsonService service = new JsonService();
            string json = "[\r\n  {\r\n    \"id\": \"2\",\r\n    \"userName\": \"TestUser\",\r\n    \"role\": \"Admin\",\r\n }\r\n]";

            List<User> expectedResult = new List<User>()
            {
                new User()
                {
                    Id = "2",
                    UserName = "TestUser",
                    Role = "Admin",
                }
            };

            var result = service.ExtractUsersFromJson(json);

            Assert.AreEqual(expectedResult[0].Id, result[0].Id);
            Assert.AreEqual(expectedResult[0].UserName, result[0].UserName);
            Assert.AreEqual(expectedResult[0].Role, result[0].Role);
        }

    }
}
