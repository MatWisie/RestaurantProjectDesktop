using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.IconPacks;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using System.Windows.Controls;

namespace RestaurantDesktop.ViewModel
{
    public partial class TablesViewModel : ObservableObject
    {
        private readonly IGridService _gridService;
        private readonly ITableService _tableService;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        private readonly IJsonService _jsonService;

        public TablesViewModel(IGridService gridService, ITableService tableService, IConfigurationService configurationService, IAuthService authService, IJsonService jsonService)
        {
            _gridService = gridService;
            _tableService = tableService;
            _configurationService = configurationService;
            _authService = authService;
            _jsonService = jsonService;

            BuildTableGrid();

        }

        [ObservableProperty]
        private Grid tableGrid;

        private async void BuildTableGrid()
        {
            MessageService.SendLoadingBegin();
            var dishResponse = await _gridService.GetGrid(_configurationService.GetConfiguration("UserToken"));
            var tablesResponse = await _tableService.GetTables(_configurationService.GetConfiguration("UserToken"));
            _authService.CheckIfLogout(dishResponse.StatusCode);
            _authService.CheckIfLogout(tablesResponse.StatusCode);
            if (dishResponse.IsSuccessful && dishResponse.Content != null && tablesResponse.IsSuccessful && tablesResponse.Content != null)
            {
                TableGridModel gridData = _jsonService.ExtractTableGridDataFromJson(dishResponse.Content);
                TableGrid = _gridService.BuildGrid(gridData);

                List<TableWithIdModel> tables = _jsonService.ExtractTablesFromJson(tablesResponse.Content);
                foreach (var table in tables)
                {
                    PackIconBoxIcons tableIcon = new PackIconBoxIcons()
                    {
                        Kind = PackIconBoxIconsKind.RegularChair,
                        DataContext = table
                    };
                    Grid.SetRow(tableIcon, table.GridRow);
                    Grid.SetColumn(tableIcon, table.GridColumn);
                    TableGrid.Children.Add(tableIcon);
                }
            }
            MessageService.SendLoadingEnd();
        }
    }
}
