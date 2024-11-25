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
        
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.roomNumber)
            .IsUnique();
        
        modelBuilder.Entity<Room>().HasData(
            new Room
            {
                id = 1, roomNumber = 101, guestQuantity = 1, costPerNight = 2000,
                description = "Уютная комната с комфортом\n\nНаша комната оснащена удобной двуспальной кроватью, телевизором, мини-баром и бесплатным Wi-Fi. В ванной комнате — качественные туалетные принадлежности и фен. В комнате также есть кондиционер, сейф и кофемашина для вашего удобства. Идеально подходит для отдыха или работы.",
            },
            new Room
            {
                id = 2, roomNumber = 102, guestQuantity = 1, costPerNight = 1500,
                description = "Уютная комната с комфортом\n\nНаша комната оснащена удобной двуспальной кроватью, телевизором, мини-баром и бесплатным Wi-Fi. В ванной комнате — качественные туалетные принадлежности и фен. В комнате также есть кондиционер, сейф и кофемашина для вашего удобства. Идеально подходит для отдыха или работы.",
            },
            new Room
            {
                id = 3, roomNumber = 103, guestQuantity = 2, costPerNight = 5000,
                description = "Уютная комната с комфортом\n\nНаша комната оснащена удобной двуспальной кроватью, телевизором, мини-баром и бесплатным Wi-Fi. В ванной комнате — качественные туалетные принадлежности и фен. В комнате также есть кондиционер, сейф и кофемашина для вашего удобства. Идеально подходит для отдыха или работы.",
            },
            new Room
            {
                id = 4, roomNumber = 201, guestQuantity = 2, costPerNight = 3000,
                description = "Уютная комната с комфортом\n\nНаша комната оснащена удобной двуспальной кроватью, телевизором, мини-баром и бесплатным Wi-Fi. В ванной комнате — качественные туалетные принадлежности и фен. В комнате также есть кондиционер, сейф и кофемашина для вашего удобства. Идеально подходит для отдыха или работы.",
            },
            new Room
            {
                id = 5, roomNumber = 202, guestQuantity = 4, costPerNight = 4500,
                description = "Уютная комната с комфортом\n\nНаша комната оснащена удобной двуспальной кроватью, телевизором, мини-баром и бесплатным Wi-Fi. В ванной комнате — качественные туалетные принадлежности и фен. В комнате также есть кондиционер, сейф и кофемашина для вашего удобства. Идеально подходит для отдыха или работы."
                
            }
        );
        
        base.OnModelCreating(modelBuilder);
    }
    
}