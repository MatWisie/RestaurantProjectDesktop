using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Aspect;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace RestaurantDesktop.ViewModel
{
    public partial class ReservationsViewModel : ObservableObject
    {
        private readonly IReservationService _reservationService;
        private readonly IConfigurationService _configurationService;
        private readonly IJsonService _jsonService;
        private readonly IAuthService _authService;
        public ReservationsViewModel(IReservationService reservationService, IConfigurationService configurationService, IJsonService jsonService, IAuthService authService)
        {
            _reservationService = reservationService;
            _configurationService = configurationService;
            _jsonService = jsonService;
            _authService = authService;

            ReloadReservations();
        }

        [ObservableProperty]
        private ObservableCollection<ReservationModel> loadedReservations;

        [AsyncLoading]
        [RelayCommand]
        private async Task ReloadReservations()
        {
            var reservationResponse = await _reservationService.GetReservations(_configurationService.GetConfiguration("UserToken"));
            if (reservationResponse.IsSuccessful && reservationResponse.Content != null)
                LoadedReservations = new ObservableCollection<ReservationModel>(_jsonService.ExtractReservationsFromJson(reservationResponse.Content));
            _authService.CheckIfLogout(reservationResponse.StatusCode);
        }

    }
}
