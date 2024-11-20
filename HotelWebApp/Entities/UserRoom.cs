using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWebApp.Entities;

[Table("user_room")]
public class UserRoom
{
    [Key]
    [Column("id")]
    public long id { get; init; }
    
    [Required]
    [Column("room")]
    public long room_id { get; set; }
    [ForeignKey("room_id")]
    public Room room { get; set; }
    
    [Required]
    [Column("user")]
    public string user_id { get; set; }
    [ForeignKey("user_id")]
    public User user { get; set; }
    
    [Required]
    [Column("booked_from")]
    public DateTime? bookedFrom { get; set; }
    
    [Required]
    [Column("booked_until")]
    public DateTime? bookedUntil { get; set; }
    
}