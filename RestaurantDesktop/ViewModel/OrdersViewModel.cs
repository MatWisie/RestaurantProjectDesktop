using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using System.Collections.ObjectModel;

namespace RestaurantDesktop.ViewModel
{
    public partial class OrdersViewModel : ObservableObject
    {
        private readonly IOrdersService _ordersService;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        private readonly IJsonService _jsonService;
        public OrdersViewModel(IOrdersService ordersService, IConfigurationService configurationService, IAuthService authService, IJsonService jsonService)
        {
            _ordersService = ordersService;
            _configurationService = configurationService;
            _authService = authService;
            _jsonService = jsonService;
            ReloadOrders();
        }

        [ObservableProperty]
        private ObservableCollection<OrderWithId> allOrders;

        partial void OnAllOrdersChanged(ObservableCollection<OrderWithId> value)
        {
            ActiveOrders = new ObservableCollection<OrderWithId>(value.Where(e => e.Status != StatusEnum.Paid));
        }

        [ObservableProperty]
        private ObservableCollection<OrderWithId> activeOrders;

        [RelayCommand]
        private async Task ReloadOrders()
        {
            MessageService.SendLoadingBegin();
            var ordersResponse = await _ordersService.GetOrders(_configurationService.GetConfiguration("UserToken"));
            if (ordersResponse.IsSuccessful && ordersResponse.Content != null)
                AllOrders = new ObservableCollection<OrderWithId>(_jsonService.ExtractOrdersFromJson(ordersResponse.Content));
            _authService.CheckIfLogout(ordersResponse.StatusCode);
            MessageService.SendLoadingEnd();
        }
    }
}
