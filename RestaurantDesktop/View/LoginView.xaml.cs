using RestaurantDesktop.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantDesktop.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel loginViewModel)
            {
                loginViewModel.Password = UserPassword.Password;
            }
        }
    }
}
