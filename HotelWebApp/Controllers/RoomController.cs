using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HotelWebApp.Data;
using HotelWebApp.Entities;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelWebApp.Controllers;


[Controller]
[Route("[controller]")]
public class RoomController(UserManager<User> userManager, DataContext context)
    : Controller
{


    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllRooms()
    {
        try
        {
            var roomModels = context.Rooms.Select(r => new RoomModel
            (
                r.id,
                r.description,
                r.roomNumber,
                r.guestQuantity,
                r.costPerNight
            )).ToList();

            return View(roomModels);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while fetching rooms.");
            return View(new List<RoomModel>());
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult Booking(long roomId)
    {
        var room = context.Rooms.FirstOrDefault(r => r.id == roomId);
        if (room == null)
        {
            ModelState.AddModelError(string.Empty, "Комната не найдена");
            return RedirectToAction("GetAllRooms", "Room");
        }

        var bookedDates = context.UserRooms
            .Where(ur => ur.room_id == roomId)
            .Select(ur => new DateRange(
                ur.bookedFrom.ToString("yyyy-MM-dd"),
                ur.bookedUntil.AddDays(1).ToString("yyyy-MM-dd")
            )).ToList();

        var bookingViewModel = new BookingViewModel(
            roomId,
            "", "",
            bookedDates);
        
        return View(bookingViewModel);
    }
    
    
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> BookRoom(BookingViewModel model)
    {
        // Validate request
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Получены не корректные значения");
            return RedirectToAction("Booking", "Room", new { roomId = model.roomId });

        }

        // Get the current user
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Пользователь не найден");
            return RedirectToAction("Booking", "Room", new { roomId = model.roomId });

        }

        // Check if the room exists
        var room = await context.Rooms.FindAsync(model.roomId);
        if (room == null && !CheckBookDates(model))
        {
            ModelState.AddModelError(string.Empty, "Комната не найдена или забронирована на эти даты");
            return RedirectToAction("Booking", "Room", new { roomId = model.roomId });
        }

        // Create the UserRoom entry
        var userRoom = new UserRoom
        {
            user_id = user.Id,
            room_id = model.roomId,
            bookedFrom = DateTime.Parse(model.bookedFrom),
            bookedUntil = DateTime.Parse(model.bookedUntil)
        };

        // Save to the database
        context.UserRooms.Add(userRoom);
        await context.SaveChangesAsync();

        return RedirectToAction("Booking", "Room", new { roomId = model.roomId });
    }


    private static bool CheckBookDates(BookingViewModel model)
    {
        if ( !(DateTime.Parse(model.bookedFrom) >= DateTime.Now &&
            DateTime.Parse(model.bookedFrom) <= DateTime.Parse(model.bookedUntil)))
        {
            return false;
        }

        if (model.bookedDates == null)
        {
            return true;
        }

        return !model.bookedDates.Any(range =>
        {
            var bookedStart = DateTime.Parse(range.from);
            var bookedEnd = DateTime.Parse(range.to).AddDays(-1);

            return
                (DateTime.Parse(model.bookedFrom) >= bookedStart && DateTime.Parse(model.bookedFrom) < bookedEnd) || 
                (DateTime.Parse(model.bookedUntil) > bookedStart && DateTime.Parse(model.bookedUntil) <= bookedEnd) ||  
                (DateTime.Parse(model.bookedFrom) < bookedStart && DateTime.Parse(model.bookedUntil) > bookedEnd);    
        });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("[action]")]
    public IActionResult AddRoom()
    {
        return View();
    }
    
    
    [HttpPost("[action]")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddRoom(AddRoomModel model)
    {
        if (ModelState.IsValid)
        {
            var newRoom = new Room
            {
                roomNumber = model.roomNumber,
                guestQuantity = model.guests,
                costPerNight = model.cost,
                description = model.description
            };

            context.Rooms.Add(newRoom);
            await context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Комната успешно добавлена!";
            return RedirectToAction("GetAllRooms");
        }

        TempData["ErrorMessage"] = "Произошла ошибка при добавлении комнаты.";
        return View(model);
    }
    
    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> UserBookings()
    {
        List<BookingView> bookings;
        
        if (User.IsInRole("Admin"))
        {
            bookings = await context.UserRooms
                .Select(ur => new BookingView
                {
                    id = ur.id,
                    roomNumber = ur.room.roomNumber,
                    cost = ur.room.costPerNight,
                    bookedFrom = ur.bookedFrom,
                    bookedUntil = ur.bookedUntil
                })
                .ToListAsync();
            return View(bookings);
        }
        
        var userId = userManager.GetUserId(User);
        bookings = await context.UserRooms
            .Where(ur => ur.user_id == userId)
            .Select(ur => new BookingView
            {
                id = ur.id,
                roomNumber = ur.room.roomNumber,
                cost = ur.room.costPerNight,
                bookedFrom = ur.bookedFrom,
                bookedUntil = ur.bookedUntil
            }).ToListAsync();

        return View(bookings);
    }


    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> DeleteBooking(long bookingId)
    {
   
        var userId = userManager.GetUserId(User);
        var booking = await context.UserRooms.FirstOrDefaultAsync(ur => ur.id == bookingId);
    
        // Validate that the booking exists
        if (booking == null)
        {
            TempData["ErrorMessage"] = "Бронь не найдена.";
            return RedirectToAction("UserBookings");
        }
    
        // Check if the user has permission to delete the booking
        if (!User.IsInRole("Admin") && booking.user_id != userId)
        {
            TempData["ErrorMessage"] = "Вы не можете удалить эту бронь.";
            return RedirectToAction("UserBookings");
        }
    
        // Remove the booking
        context.UserRooms.Remove(booking);

        try
        {
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Бронь успешно удалена.";
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Не удалось удалить бронь. Попробуйте позже.";
        }

        return RedirectToAction("UserBookings");
    }
        
}