namespace HotelWebApp.Models;

public class BookingViewModel
{
    public long roomId { get; init; }
    public string bookedFrom { get; set; }
    public string bookedUntil { get; set; }
    public List<DateRange>? bookedDates { get; set; }

    public BookingViewModel()
    {
        
    }

    public BookingViewModel(long roomId, string bookedFrom, string bookedUntil, List<DateRange>? bookedDates)
    {
        this.roomId = roomId;
        this.bookedFrom = bookedFrom;
        this.bookedUntil = bookedUntil;
        this.bookedDates = bookedDates;
    }
}