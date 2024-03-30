using RestaurantDesktop.Model;
using RestSharp;
using System.Windows.Controls;

namespace RestaurantDesktop.Interface
{
    public interface IGridService
    {
        RestResponse EditGrid(string userToken, TableGridModel tableEditModel);
        Task<RestResponse> GetGrid(string userToken);
        Grid BuildGrid(TableGridModel gridData);
        Rectangle BuildDropRectangle();
    }
}