using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class EditTableViewModel : ObservableObject
    {
        private readonly TableWithIdModel _tableWithId;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        private readonly ITableService _tableService;
        public EditTableViewModel(TableWithIdModel table, IConfigurationService configurationService, IAuthService authService, ITableService tableService)
        {
            _tableWithId = table;
            _configurationService = configurationService;
            _authService = authService;
            _tableService = tableService;

            NumberOfSeats = _tableWithId.NumberOfSeats;
            IsAvailable = _tableWithId.IsAvailable;
        }
        [ObservableProperty]
        private int numberOfSeats;

        [ObservableProperty]
        private bool isAvailable;

        [ObservableProperty]
        private string errorText;

        [RelayCommand]
        private async Task EditTable()
        {
            if (CheckTableValues(NumberOfSeats))
            {
                return;
            }
            string token = _configurationService.GetConfiguration("UserToken");
            TableModel tableWithIdModel = new TableModel()
            {
                IsAvailable = IsAvailable,
                NumberOfSeats = NumberOfSeats,
                GridRow = _tableWithId.GridRow,
                GridColumn = _tableWithId.GridColumn,
            };
            var result = await _tableService.EditTable(token, tableWithIdModel, _tableWithId.Id);
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

        private bool CheckTableValues(int seats)
        {
            if (seats == 0)
            {
                ErrorText = "Please complete all fields";
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
