namespace HotelWebApp.Models;

public class BookRoomRequest
{
    public int roomId { get; set; }
    public DateTime bookedFrom { get; set; }
    public DateTime bookedUntil { get; set; }
}