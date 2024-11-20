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
public class UserController(DataContext context) : Controller
{
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var userModels = context.Users.Select(u => new UserModel
            (
                u.UserName,
                u.createdAt,
                context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Select(ur => ur.RoleId)
                    .Join(context.Roles, 
                        roleId => roleId, 
                        r => r.Id,
                        (roleId, r) => r.Name)
                    .FirstOrDefault()
            )).ToList();

            return View(userModels);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while fetching users.");
            return View(new List<UserModel>());
        }
    }
        
}