using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestSharp;
using System.Text.Json;

namespace RestaurantDesktop.Service
{
    public class TableService : ITableService
    {
        private readonly ITablesRepository _tablesRepository;
        public TableService(ITablesRepository tablesRepository)
        {
            _tablesRepository = tablesRepository;
        }
        public async Task<RestResponse> GetTables(string userToken)
        {
            return await _tablesRepository.GetTables(userToken);
        }

        public RestResponse AddTable(string userToken, TableModel tableModel)
        {
            string json = JsonSerializer.Serialize(tableModel);
            return _tablesRepository.AddTable(userToken, json);
        }

        public async Task<RestResponse> EditTable(string userToken, TableModel tableEditModel, string tableIdToEdit)
        {
            string json = JsonSerializer.Serialize(tableEditModel);
            return await _tablesRepository.EditTable(userToken, json, tableIdToEdit);
        }

        public async Task<RestResponse> DeleteTable(string userToken, string tableIdToDelete)
        {
            return await _tablesRepository.DeleteTable(userToken, tableIdToDelete);
        }

        public TableWithIdModel CopyTableModel(TableWithIdModel model)
        {
            return new TableWithIdModel()
            {
                Id = model.Id,
                IsAvailable = model.IsAvailable,
                NumberOfSeats = model.NumberOfSeats,
                GridColumn = model.GridColumn,
                GridRow = model.GridRow,

            };
        }
    }
}
