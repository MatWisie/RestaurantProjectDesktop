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


        }
    }
}
