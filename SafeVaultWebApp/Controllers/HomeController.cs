using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SafeVaultWebApp.Models;
using SafeVaultWebApp.Data;
using SafeVaultWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace SafeVaultWebApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly SafeVaultDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(SafeVaultDbContext safeVaultDbContext, UserManager<User> userManager)
        {
            _context = safeVaultDbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Basic authentication logic (replace with secure password hashing in production)
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || password != "testpassword") // Replace with real password check
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }
            // Set authentication cookie or session here (not implemented for brevity)
            return RedirectToAction("UserProfile");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            (var isValid, string sanitizedUsername, string sanitizedEmail, string error) = ValidationHelpers.ValidateUserInput(username, email);
            if (!isValid)
            {
                ViewBag.Error = error;
                return BadRequest("Invalid Username");
            }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                ViewBag.Error = "Password must be at least 6 characters.";
                return View();
            }
            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            // Save to database
            var user = new User { Username = sanitizedUsername, UserName = sanitizedUsername, Email = sanitizedEmail, PasswordHash = passwordHash };
            var result = await _userManager.CreateAsync(user, password);
            ViewBag.Success = "User registered successfully.";
            return RedirectToAction("UserProfile");
        }

        public IActionResult UserProfile()
        {
            ViewBag.Username = _userManager.GetUserName(User);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 