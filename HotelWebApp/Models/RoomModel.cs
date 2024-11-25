namespace HotelWebApp.Models;

public record RoomModel(long id, string description, int roomNumber, int guestQuantity, decimal costPerNight);