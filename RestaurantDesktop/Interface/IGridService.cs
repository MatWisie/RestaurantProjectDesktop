using MahApps.Metro.IconPacks;
using RestaurantDesktop.Model;
using RestSharp;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace RestaurantDesktop.Interface
{
    public interface IGridService
    {
        RestResponse EditGrid(string userToken, TableGridModel tableEditModel);
        Task<RestResponse> GetGrid(string userToken);
        Grid BuildGrid(TableGridModel gridData);
        Rectangle BuildDropRectangle();
        PackIconBoxIcons BuildPackIcon(TableWithIdModel? table);
    }
}