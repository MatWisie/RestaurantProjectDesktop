using RestaurantDesktop.Model;
using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface ITableService
    {
        RestResponse AddTable(string userToken, TableModel tableModel);
        Task<RestResponse> DeleteTable(string userToken, string tableIdToDelete);
        Task<RestResponse> EditTable(string userToken, TableModel tableEditModel, string tableIdToEdit);
        Task<RestResponse> GetTables(string userToken);
        TableWithIdModel CopyTableModel(TableWithIdModel model);
    }
}