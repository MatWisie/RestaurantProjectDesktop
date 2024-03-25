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

        [RelayCommand]
        private void AddTable()
        {
            if (CheckNullTableValues(NumberOfSeats))
            {
                return;
            }
            if (CheckGrids(GridRow, GridColumn))
            {
                return;
            }
            string token = _configurationService.GetConfiguration("UserToken");

            TableModel tableAddModel = new TableModel()
            {
                IsAvailable = IsAvailable,
                NumberOfSeats = NumberOfSeats,
                GridRow = GridRow,
                GridColumn = GridColumn,
            };
            var result = _tableService.AddTable(token, tableAddModel);
            _authService.CheckIfLogout(result.StatusCode);
            if (result.IsSuccessful)
            {
                ReturnToPreviousView();
            }
            else
            {
                ErrorText = string.IsNullOrEmpty(result.ErrorMessage) ? result.Content : result.ErrorMessage;
            }
        }
        private bool CheckNullTableValues(int numberOfSeats)
        {
            if (numberOfSeats == 0)
            {
                ErrorText = "Please complete all fields";
                return true;
            }
            return false;
        }

        private bool CheckGrids(int gridRow, int gridColumn)
        {
            if (gridRow > _gridModel.NumberOfRows || gridColumn > _gridModel.NumberOfColumns || gridRow < 0 || gridColumn < 0)
            {
                ErrorText = "Please complete all fields";
                return true;
            }
            if (_tables.Any(e => e.GridRow == gridRow && e.GridColumn == gridColumn))
            {
                ErrorText = "There already is table on this location";
                return true;
            }
            return false;
        }

        [RelayCommand]
        private void ReturnToTablesViewModel()
        {
            ReturnToPreviousView();
        }

        private void ReturnToPreviousView()
        {
            WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<TablesViewModel>()));
        }

    }
}
