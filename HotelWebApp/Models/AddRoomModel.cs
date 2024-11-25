namespace HotelWebApp.Models;

public class AddRoomModel
{
    public int roomNumber { get; set; } 
    public int guests { get; set; } 
    public decimal cost { get; set; }   
    public string description { get; set; }
}