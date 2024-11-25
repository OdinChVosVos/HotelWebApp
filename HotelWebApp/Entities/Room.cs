using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWebApp.Entities;

[Table("room")]
public class Room
{
    [Key]
    [Column("id")]
    public long id { get; init; }
    
    [Required]
    [Column("room_number")]
    public int roomNumber { get; set; }
    
    [Required]
    [Column("description")]
    public string description { get; set; }
    
    [Required]
    [Column("guest_quantity")]
    public int guestQuantity { get; set; }
    
    [Required]
    [Column("cost_per_night")]
    public decimal costPerNight { get; set; }


}