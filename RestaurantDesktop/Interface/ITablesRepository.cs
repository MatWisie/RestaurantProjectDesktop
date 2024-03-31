using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface ITablesRepository
    {
        RestResponse AddTable(string userToken, string json);
        Task<RestResponse> DeleteTable(string userToken, string tableIdToDelete);
        Task<RestResponse> EditTable(string userToken, string json, string tableIdToEdit);
        Task<RestResponse> GetTables(string userToken);
    }
}