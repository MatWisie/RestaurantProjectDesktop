using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.IconPacks;
using Microsoft.Extensions.DependencyInjection;
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

        [ObservableProperty]
        private PackIconControlBase selectedTable;

        private TableGridModel tableGridModel;
        private List<TableWithIdModel> tables;

        [RelayCommand]
        private async Task BuildTableGrid()
        {
            MessageService.SendLoadingBegin();
            var dishResponse = await _gridService.GetGrid(_configurationService.GetConfiguration("UserToken"));
            var tablesResponse = await _tableService.GetTables(_configurationService.GetConfiguration("UserToken"));
            _authService.CheckIfLogout(dishResponse.StatusCode);
            _authService.CheckIfLogout(tablesResponse.StatusCode);
            if (dishResponse.IsSuccessful && dishResponse.Content != null && tablesResponse.IsSuccessful && tablesResponse.Content != null)
            {
                tableGridModel = _jsonService.ExtractTableGridDataFromJson(dishResponse.Content);
                TableGrid = _gridService.BuildGrid(tableGridModel);

                tables = _jsonService.ExtractTablesFromJson(tablesResponse.Content);
                foreach (var table in tables)
                {
                    PackIconBoxIcons tableIcon = new PackIconBoxIcons()
                    {
                        Kind = PackIconBoxIconsKind.RegularChair,
                        DataContext = table,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        VerticalAlignment = System.Windows.VerticalAlignment.Stretch
                    };
                    Grid.SetRow(tableIcon, table.GridRow);
                    Grid.SetColumn(tableIcon, table.GridColumn);
                    TableGrid.Children.Add(tableIcon);
                }
            }
            MessageService.SendLoadingEnd();
        }

        [RelayCommand]
        private void GoToAddTable()
        {
            MessageService.SendChangeViewMessage(new AddTableViewModel(tableGridModel, tables, App.Current.Services.GetService<IConfigurationService>(), App.Current.Services.GetService<IAuthService>(), App.Current.Services.GetService<ITableService>())); //App.Current.Services.GetService<AddTableViewModel>());
        }

        [RelayCommand]
        private void GoToMainMenu()
        {
            MessageService.SendChangeViewMessage(App.Current.Services.GetService<MainMenuViewModel>());
        }
    }
}
