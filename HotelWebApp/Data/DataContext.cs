using HotelWebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotelWebApp.Data;

public class DataContext : IdentityDbContext<User>
{
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<UserRoom> UserRooms { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Room>().HasData(
            new Room
            {
                id = 1, roomNumber = 101, guestQuantity = 1, costPerNight = 2000,
                description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis a mi gravida, vulputate nibh non, rutrum nisl. Duis mattis quam ut erat cursus blandit. Aenean volutpat ut felis sed gravida. Ut semper euismod nisi et euismod. Mauris sollicitudin eleifend vestibulum. Ut nec nisl quis nulla tincidunt aliquam. Aenean pellentesque ultrices ante, ut euismod orci luctus nec. Quisque hendrerit semper purus, ut sodales dui tempus vitae. Mauris efficitur porta sem, vitae finibus felis euismod in.",
            },
            new Room
            {
                id = 2, roomNumber = 102, guestQuantity = 1, costPerNight = 1500,
                description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis a mi gravida, vulputate nibh non, rutrum nisl. Duis mattis quam ut erat cursus blandit. Aenean volutpat ut felis sed gravida. Ut semper euismod nisi et euismod. Mauris sollicitudin eleifend vestibulum. Ut nec nisl quis nulla tincidunt aliquam. Aenean pellentesque ultrices ante, ut euismod orci luctus nec. Quisque hendrerit semper purus, ut sodales dui tempus vitae. Mauris efficitur porta sem, vitae finibus felis euismod in.",
            },
            new Room
            {
                id = 3, roomNumber = 103, guestQuantity = 2, costPerNight = 5000,
                description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis a mi gravida, vulputate nibh non, rutrum nisl. Duis mattis quam ut erat cursus blandit. Aenean volutpat ut felis sed gravida. Ut semper euismod nisi et euismod. Mauris sollicitudin eleifend vestibulum. Ut nec nisl quis nulla tincidunt aliquam. Aenean pellentesque ultrices ante, ut euismod orci luctus nec. Quisque hendrerit semper purus, ut sodales dui tempus vitae. Mauris efficitur porta sem, vitae finibus felis euismod in.",
            },
            new Room
            {
                id = 4, roomNumber = 201, guestQuantity = 2, costPerNight = 3000,
                description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis a mi gravida, vulputate nibh non, rutrum nisl. Duis mattis quam ut erat cursus blandit. Aenean volutpat ut felis sed gravida. Ut semper euismod nisi et euismod. Mauris sollicitudin eleifend vestibulum. Ut nec nisl quis nulla tincidunt aliquam. Aenean pellentesque ultrices ante, ut euismod orci luctus nec. Quisque hendrerit semper purus, ut sodales dui tempus vitae. Mauris efficitur porta sem, vitae finibus felis euismod in.",
            },
            new Room
            {
                id = 5, roomNumber = 202, guestQuantity = 4, costPerNight = 4500,
                description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis a mi gravida, vulputate nibh non, rutrum nisl. Duis mattis quam ut erat cursus blandit. Aenean volutpat ut felis sed gravida. Ut semper euismod nisi et euismod. Mauris sollicitudin eleifend vestibulum. Ut nec nisl quis nulla tincidunt aliquam. Aenean pellentesque ultrices ante, ut euismod orci luctus nec. Quisque hendrerit semper purus, ut sodales dui tempus vitae. Mauris efficitur porta sem, vitae finibus felis euismod in.",
            }
        );
        
        base.OnModelCreating(modelBuilder);
    }
    
}