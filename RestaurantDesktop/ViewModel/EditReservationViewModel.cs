using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class EditReservationViewModel : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        private readonly IReservationService _reservationService;
        public EditReservationViewModel(ReservationModel reservation, IConfigurationService configurationService, IAuthService authService, IReservationService reservationService)
        {
            _configurationService = configurationService;
            _authService = authService;
            _reservationService = reservationService;

            From = reservation.From;
            To = reservation.To;
            id = reservation.Id;
        }
        private readonly string id;

        [ObservableProperty]
        private DateTime from;

        [ObservableProperty]
        private DateTime to;

        [ObservableProperty]
        private string errorText;

        [RelayCommand]
        private async Task EditReservation()
        {
            if (!CheckReservation())
            {
                return;
            }
            string token = _configurationService.GetConfiguration("UserToken");
            ReservationEditModel reservationEditModel = new ReservationEditModel()
            {
                From = From,
                To = To,
            };
            var result = await _reservationService.EditReservation(token, reservationEditModel, id);
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

        private bool CheckReservation()
        {
            bool result = From > DateTime.Now && To > DateTime.Now && To > From;
            if (!result)
            {
                ErrorText = "Please fill in the correct dates";
                return result;
            }
            return result;
        }

        [RelayCommand]
        private void ReturnToPreviousView()
        {
            WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<ReservationsViewModel>()));
        }
    }
}
