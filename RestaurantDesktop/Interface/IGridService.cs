using MahApps.Metro.IconPacks;
using RestaurantDesktop.Model;
using RestSharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace RestaurantDesktop.Interface
{
    public interface IGridService
    {
        RestResponse EditGrid(string userToken, TableGridModel tableEditModel);
        Task<RestResponse> GetGrid(string userToken);
        Grid BuildGrid(TableGridModel gridData, List<TableWithIdModel> tables);
        Rectangle BuildDropRectangle();
        PackIconBoxIcons BuildPackIcon(TableWithIdModel? table);
        bool SetElementOnGrid(FrameworkElement element, Grid grid, int row, int column);
    }
}