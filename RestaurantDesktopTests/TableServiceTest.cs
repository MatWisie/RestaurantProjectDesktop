using Moq;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using RestSharp;

namespace RestaurantDesktopTests
{
    [TestClass]
    public class TableServiceTest
    {
        private Mock<ITablesRepository> _mockRepository;
        private TableService _tableService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<ITablesRepository>();
            _tableService = new TableService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetTables_MockRepository_ShouldReturnRestResponse()
        {
            string userToken = "userToken";
            var expectedResponse = new RestResponse();

            _mockRepository.Setup(r => r.GetTables(userToken)).ReturnsAsync(expectedResponse);

            var result = await _tableService.GetTables(userToken);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public void AddTable_MockRepository_ShouldReturnRestResponse()
        {
            string userToken = "userToken";
            var tableModel = new TableModel();
            var expectedResponse = new RestResponse();

            _mockRepository.Setup(r => r.AddTable(userToken, It.IsAny<string>())).Returns(expectedResponse);

            var result = _tableService.AddTable(userToken, tableModel);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task EditTable_MockRepository_ShouldReturnRestResponse()
        {
            string userToken = "userToken";
            string tableIdToEdit = "testTableId";
            var tableEditModel = new TableModel();
            var expectedResponse = new RestResponse();

            _mockRepository.Setup(r => r.EditTable(userToken, It.IsAny<string>(), tableIdToEdit)).ReturnsAsync(expectedResponse);

            var result = await _tableService.EditTable(userToken, tableEditModel, tableIdToEdit);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task DeleteTable_MockRepository_ShouldReturnRestResponse()
        {
            string userToken = "userToken";
            string tableIdToDelete = "testTableId";
            var expectedResponse = new RestResponse();

            _mockRepository.Setup(r => r.DeleteTable(userToken, tableIdToDelete)).ReturnsAsync(expectedResponse);

            var result = await _tableService.DeleteTable(userToken, tableIdToDelete);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public void CopyTableModel_MockRepository_ShouldReturnRestResponse()
        {
            var model = new TableWithIdModel
            {
                Id = "testId",
                IsAvailable = true,
                NumberOfSeats = 4,
                GridColumn = 2,
                GridRow = 3
            };

            var result = _tableService.CopyTableModel(model);

            Assert.AreEqual(model.Id, result.Id);
            Assert.AreEqual(model.IsAvailable, result.IsAvailable);
            Assert.AreEqual(model.NumberOfSeats, result.NumberOfSeats);
            Assert.AreEqual(model.GridColumn, result.GridColumn);
            Assert.AreEqual(model.GridRow, result.GridRow);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CopyTableModel_NullArgument_ThrowArgumentNullException()
        {
            var model = new TableWithIdModel
            {
                Id = "testId",
                IsAvailable = true,
                NumberOfSeats = 4,
                GridColumn = 2,
                GridRow = 3
            };

            var result = _tableService.CopyTableModel(null);
        }
    }
}
