namespace HotelWebApp.Models;

public record RoomModel(string description, int roomNumber, int guestQuantity, int costPerNight, bool active);