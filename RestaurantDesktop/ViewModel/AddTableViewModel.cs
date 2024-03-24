using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class AddTableViewModel : ObservableObject
    {
        private readonly TableGridModel _gridModel;
        private readonly List<TableWithIdModel> _tables;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        private readonly ITableService _tableService;
        public AddTableViewModel(TableGridModel tableGridModel, List<TableWithIdModel> tables, IConfigurationService configurationService, IAuthService authService, ITableService tableService)
        {
            _gridModel = tableGridModel;
            _tables = tables;
            _configurationService = configurationService;
            _authService = authService;
            _tableService = tableService;

        }

        public string SizeOfGrid
        {
            get => _gridModel.NumberOfColumns + "x" + _gridModel.NumberOfRows;
        }

        [ObservableProperty]
        private bool isAvailable = true;
        [ObservableProperty]
        private int numberOfSeats;
        [ObservableProperty]
        private int gridRow;
        [ObservableProperty]
        private int gridColumn;
        [ObservableProperty]
        private string errorText;


    }
}
