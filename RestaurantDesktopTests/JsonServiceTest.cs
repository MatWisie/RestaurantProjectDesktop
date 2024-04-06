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
            var result = service.ExtractOrdersFromJson(json);

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

            Assert.AreEqual(expectedResult[0].Dishes[0].id, result[0].Dishes[0].id);
            Assert.AreEqual(expectedResult[0].Dishes[0].name, result[0].Dishes[0].name);
            Assert.AreEqual(expectedResult[0].Dishes[0].description, result[0].Dishes[0].description);
            Assert.AreEqual(expectedResult[0].Dishes[0].price, result[0].Dishes[0].price);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void ExtractOrdersFromJson_InCorrectJson_CorrectDishesInOrder()
        {
            JsonService service = new JsonService();
            string json = "bla";
            var result = service.ExtractOrdersFromJson(json);

        }
    }
}
