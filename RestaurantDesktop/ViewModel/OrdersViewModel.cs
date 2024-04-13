using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Aspect;
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
        private int flipViewIndex;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ActiveOrders))]
        private ObservableCollection<OrderWithId> allOrders;

        partial void OnAllOrdersChanged(ObservableCollection<OrderWithId> value)
        {
            UpdateActiveOrders();
        }

        private void UpdateActiveOrders()
        {
            ActiveOrders = new ObservableCollection<OrderWithId>(AllOrders.Where(e => e.Status != StatusEnum.Paid));
        }

        [ObservableProperty]
        private ObservableCollection<OrderWithId> activeOrders;

        [AsyncLoading]
        [RelayCommand]
        private async Task ReloadOrders()
        {
            var ordersResponse = await _ordersService.GetOrders(_configurationService.GetConfiguration("UserToken"));
            if (ordersResponse.IsSuccessful && ordersResponse.Content != null)
            {
                AllOrders = new ObservableCollection<OrderWithId>(_jsonService.ExtractOrdersFromJson(ordersResponse.Content));
                FlipViewIndex = 0;
            }
            _authService.CheckIfLogout(ordersResponse.StatusCode);
        }

        private async Task ChangeStatusToReady(OrderWithId? order)
        {
            if (order != null)
            {
                var ordersResponse = await _ordersService.ChangeStatusReady(_configurationService.GetConfiguration("UserToken"), order.Id);
                if (ordersResponse.IsSuccessful && ordersResponse.Content != null)
                {
                    order.Status = StatusEnum.Ready;
                    OnPropertyChanged(nameof(AllOrders));
                }
                _authService.CheckIfLogout(ordersResponse.StatusCode);
            }
        }

        private async Task ChangeStatusToPaid(OrderWithId? order)
        {
            if (order != null)
            {
                var ordersResponse = await _ordersService.ChangeStatusPaid(_configurationService.GetConfiguration("UserToken"), order.Id);
                if (ordersResponse.IsSuccessful && ordersResponse.Content != null)
                {
                    if (order != null)
                    {
                        order.Status = StatusEnum.Paid;
                        OnPropertyChanged(nameof(AllOrders));
                    }
                }
                _authService.CheckIfLogout(ordersResponse.StatusCode);
            }
        }

        [AsyncLoading]
        [RelayCommand]
        private async Task ChangeStatus(int orderId)
        {
            var order = ActiveOrders.FirstOrDefault(e => e.Id == orderId);
            if (order != null)
            {
                if (order.Status == StatusEnum.Working)
                {
                    await ChangeStatusToReady(order);
                }
                else if (order.Status == StatusEnum.ReadyToPay)
                {
                    await ChangeStatusToPaid(order);
                }
            }
        }

        [AsyncLoading]
        [RelayCommand]
        private async Task DeleteOrder(int orderId)
        {
            var ordersResponse = await _ordersService.DeleteOrder(_configurationService.GetConfiguration("UserToken"), orderId);
            if (ordersResponse.IsSuccessful && ordersResponse.Content != null)
            {
                var order = ActiveOrders.FirstOrDefault(e => e.Id == orderId);
                if (order != null)
                {
                    AllOrders.Remove(order);
                    UpdateActiveOrders();
                    FlipViewIndex = 0;
                }
            }
            _authService.CheckIfLogout(ordersResponse.StatusCode);
        }

        [RelayCommand]
        private void GoToMainMenu()
        {
            MessageService.SendChangeViewMessage(App.Current.Services.GetService<MainMenuViewModel>());
        }
    }
}
