using RestaurantDesktop.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantDesktop.View
{
    /// <summary>
    /// Interaction logic for UserChangePasswordView.xaml
    /// </summary>
    public partial class UserChangePasswordView : UserControl
    {
        public UserChangePasswordView()
        {
            InitializeComponent();
        }

        private void OldUserPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserChangePasswordViewModel editUserViewModel)
            {
                editUserViewModel.CurrentPassword = OldUserPassword.Password;
            }
        }

        private void NewUserPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserChangePasswordViewModel editUserViewModel)
            {
                editUserViewModel.NewPassword = NewUserPassword.Password;
            }
        }

        private void ConfirmUserPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserChangePasswordViewModel editUserViewModel)
            {
                editUserViewModel.ConfirmPassword = ConfirmUserPassword.Password;
            }
        }
    }
}
