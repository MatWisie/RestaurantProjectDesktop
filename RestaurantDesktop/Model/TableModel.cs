namespace RestaurantDesktop.Model
{
    public class TableModel
    {
        public bool IsAvailable { get; set; } = true;
        public int NumberOfSeats { get; set; }
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
    }
}
