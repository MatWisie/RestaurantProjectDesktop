using CommunityToolkit.Mvvm.ComponentModel;

namespace RestaurantDesktop.Model.Message
{
    public class ChangeMainViewMessage
    {
        public ChangeMainViewMessage(ObservableObject viewModel)
        {
            ViewModel = viewModel;
        }
        public ObservableObject ViewModel { get; set; }
    }
}
