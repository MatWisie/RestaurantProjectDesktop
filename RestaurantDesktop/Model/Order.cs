using CommunityToolkit.Mvvm.ComponentModel;

namespace RestaurantDesktop.Model
{
    public partial class Order : ObservableObject
    {
        [ObservableProperty]
        public StatusEnum status;
        public double Price { get; set; }
        public List<DishWithIdModel> Dishes { get; set; }
        public int TableId { get; set; }
        public string UserId { get; set; }
    }
}
