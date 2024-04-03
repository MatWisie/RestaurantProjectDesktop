using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;
using System.Windows;

namespace RestaurantDesktop.ViewModel
{
    public partial class EditInfrastructureViewModel : ObservableObject
    {
        private readonly TableGridModel _gridModel;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        private readonly IGridService _gridService;
        public EditInfrastructureViewModel(TableGridModel tableGridModel, IConfigurationService configurationService, IAuthService authService, IGridService gridService)
        {
            _gridModel = tableGridModel;
            _configurationService = configurationService;
            _authService = authService;
            _gridService = gridService;

            GridRows = _gridModel.NumberOfRows;
            GridColumns = _gridModel.NumberOfColumns;
        }
        [ObservableProperty]
        private int gridRows;
        [ObservableProperty]
        private int gridColumns;
        [ObservableProperty]
        private string errorText;

        [RelayCommand]
        private void EditInfrastructure()
        {
            if (MessageBox.Show("Are you sure you want to change the infrastructure? This will erase all tables in restaurant and all objects related to them.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (CheckNullTableValues(GridRows, GridColumns))
                {
                    return;
                }

                string token = _configurationService.GetConfiguration("UserToken");

                TableGridModel gridModel = new TableGridModel()
                {
                    NumberOfRows = GridRows,
                    NumberOfColumns = GridColumns
                };
                var result = _gridService.EditGrid(token, gridModel);
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
        }
        private bool CheckNullTableValues(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
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
