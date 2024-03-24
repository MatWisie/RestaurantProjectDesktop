using RestaurantDesktop.Model;
using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface ITableService
    {
        RestResponse AddTable(string userToken, TableModel tableModel);
        Task<RestResponse> DeleteTable(string userToken, string tableIdToDelete);
        RestResponse EditTable(string userToken, TableModel tableEditModel, string tableIdToEdit);
        Task<RestResponse> GetTables(string userToken);
    }
}