using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;

public class HelloController : Controller{
    

    public string Index()
    {
        return "Hi man";
    }

    public string Friend()
    {
        return "Who is your friend?";
    }
}