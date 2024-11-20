using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HotelWebApp.Data;
using HotelWebApp.Entities;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;


public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    
    
    public AccountController(UserManager<User> user, SignInManager<User> signIn)
    {
        _userManager = user;
        _signInManager = signIn;
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var usr = await _userManager.FindByEmailAsync(model.email);
            if (usr is null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден");
                return View();
            }
            
            var res = await _signInManager.PasswordSignInAsync(usr, model.password, false, false);

            if (res.Succeeded)
            {
                await Authenticate(model.email);
                return RedirectToAction("Index", "Home");
            }
            
            
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View();
        }
        
        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        return View();
    }
    
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            await Authenticate(model.email);

            var usr = new User
            {
                Email = model.email,
                UserName = model.fullName,
                fullName = model.fullName,
            };
            
            var res = await _userManager.CreateAsync(usr, model.password);
                
            if (res.Succeeded)
            {
                await  _userManager.AddToRoleAsync(usr, "User");
                await _signInManager.SignInAsync(usr, isPersistent: false);
                
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError("", "Имя пользователя уже занято");
            return View();
        }
        
        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        return View();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
    
    
    
    private async Task Authenticate(string email)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, email)
        };
            
        var id = new ClaimsIdentity(
            claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(new ClaimsPrincipal(id));
    }
    
    
    private string GetHash(string password)
    {
        var salt = Environment.GetEnvironmentVariable("SALT");
        var md5 = MD5.Create();
            
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
        Console.WriteLine(Convert.ToBase64String(hash));
        return Convert.ToBase64String(hash);
    }
        
}