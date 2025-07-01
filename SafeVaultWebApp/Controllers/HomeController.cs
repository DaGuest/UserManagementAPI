using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SafeVaultWebApp.Models;
using SafeVaultWebApp.Data;
using SafeVaultWebApp.Helpers;

namespace SafeVaultWebApp.Controllers;

public class HomeController : Controller
{
    private readonly SafeVaultDbContext _context;

    public HomeController(SafeVaultDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Submit(string username, string email)
    {
        (var isValid, string sanitizedUsername, string sanitizedEmail, string error) = ValidationHelpers.ValidateUserInput(username, email);
        if (!isValid)
        {
            // Return error message to the view
            return BadRequest(error);
        }
        // Save to database
        var user = new User { Username = sanitizedUsername, Email = sanitizedEmail };
        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok($"User saved: Username: {sanitizedUsername}, Email: {sanitizedEmail}");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
