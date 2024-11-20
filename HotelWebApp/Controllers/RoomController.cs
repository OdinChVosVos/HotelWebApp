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

namespace HotelWebApp.Controllers;


[Controller]
[Route("[controller]")]
public class RoomController(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context)
    : Controller
{


    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> GetAllRooms()
    {
        try
        {
            var roomModels = context.Rooms.Select(r => new RoomModel
            (
                r.description,
                r.roomNumber,
                r.guestQuantity,
                r.costPerNight,
                r.active
            )).ToList();

            return View(roomModels);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while fetching rooms.");
            return View(new List<RoomModel>());
        }
    }
    
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> BookRoom([FromBody] BookRoomRequest model)
    {
        // Validate request
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Get the current user
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized("User not found.");
        }

        // Check if the room exists
        var room = await context.Rooms.FindAsync(model.roomId);
        if (room == null || !room.active)
        {
            return NotFound("Room is not found or is already booked");
        }

        // Create the UserRoom entry
        var userRoom = new UserRoom
        {
            user_id = user.Id,
            room_id = model.roomId,
            bookedFrom = model.bookedFrom,
            bookedUntil = model.bookedUntil
        };

        // Save to the database
        context.UserRooms.Add(userRoom);
        await context.SaveChangesAsync();

        return Ok(new { Message = "Room booked successfully!" });
    }
    
        
}