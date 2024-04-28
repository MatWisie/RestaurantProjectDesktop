namespace RestaurantDesktop.Model
{
    public class LoginLogModel
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public bool Success { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StatusCode { get; set; }
        public string? IdentityUserId { get; set; }
    }
}
