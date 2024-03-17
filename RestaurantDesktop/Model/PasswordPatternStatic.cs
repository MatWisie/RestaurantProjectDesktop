using System.Text.RegularExpressions;

namespace RestaurantDesktop.Model
{
    public static class PasswordPatternStatic
    {
        public static Regex PasswordPattern => new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
    }
}
