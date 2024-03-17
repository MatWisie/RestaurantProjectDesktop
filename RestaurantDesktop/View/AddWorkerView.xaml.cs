using RestaurantDesktop.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantDesktop.View
{
    /// <summary>
    /// Interaction logic for AddWorkerView.xaml
    /// </summary>
    public partial class AddWorkerView : UserControl
    {
        public AddWorkerView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddWorkerViewModel addWorkerViewModel)
            {
                addWorkerViewModel.Password = UserPassword.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddWorkerViewModel addWorkerViewModel)
            {
                addWorkerViewModel.ConfirmPassword = ConfirmUserPassword.Password;
            }
        }
    }
}
