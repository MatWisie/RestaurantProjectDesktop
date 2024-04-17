namespace RestaurantDesktop.Model
{
    public class ReservationModel
    {
        public string Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string UserId { get; set; }
        public int TableId { get; set; }
    }
}
