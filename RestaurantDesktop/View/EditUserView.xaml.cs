using RestaurantDesktop.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantDesktop.View
{
    /// <summary>
    /// Interaction logic for EditUserView.xaml
    /// </summary>
    public partial class EditUserView : UserControl
    {
        public EditUserView()
        {
            InitializeComponent();
        }

        private void UserPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditUserViewModel editUserViewModel)
            {
                editUserViewModel.Password = UserPassword.Password;
            }
        }

        private void ConfirmUserPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditUserViewModel editUserViewModel)
            {
                editUserViewModel.ConfirmPassword = ConfirmUserPassword.Password;
            }
        }
    }
}
