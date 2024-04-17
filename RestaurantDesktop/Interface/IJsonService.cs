using RestaurantDesktop.Model;

namespace RestaurantDesktop.Interface
{
    public interface IJsonService
    {
        string ExtractFromJson(string json, string key);
        List<User> ExtractUsersFromJson(string json);
        List<DishWithIdModel> ExtractDishesFromJson(string json);
        TableGridModel ExtractTableGridDataFromJson(string json);
        List<TableWithIdModel> ExtractTablesFromJson(string json);
        List<OrderWithId> ExtractOrdersFromJson(string json);
        List<ReservationModel> ExtractReservationsFromJson(string json);
    }
}