namespace RestaurantDesktop.Model
{
    public class Order
    {
        public StatusEnum Status { get; set; }
        public double Price { get; set; }
        public List<DishWithIdModel> Dishes { get; set; }
        public int TableId { get; set; }
        public string UserId { get; set; }
    }
}
