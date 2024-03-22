using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.Service
{
    public static class MessageService
    {
        public static void SendLoadingBegin()
        {
            WeakReferenceMessenger.Default.Send(new LoadingBeginMessage());
        }
        public static void SendLoadingEnd()
        {
            WeakReferenceMessenger.Default.Send(new LoadingEndMessage());
        }

        public static void SendChangeViewMessage(ObservableObject vm)
        {
            WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(vm));
        }
    }
}
