using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PinkCare.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using PinkCare.Models;

namespace PinkCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PinkCareDBContext _context;
        // Update the constructor to accept PinkCareDBContext as a parameter and assign it to _context
        public HomeController(ILogger<HomeController> logger, PinkCareDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        // register page
        public IActionResult register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> register(UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //checking if email already exists
                    var existingUser = await _context.UserInfos
                   .FirstOrDefaultAsync(u => u.Email == userInfo.Email);

                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View(userInfo);
                    }
                    // hashing password
                    userInfo.PasswordHash = HashPassword(userInfo.PasswordHash);
                    userInfo.CreatedAt = DateTime.Now;

                    _context.Add(userInfo);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Registration successful! Please log in.";
                    return RedirectToAction("login");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "An error occurred while registering a new user.");
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
                }
            }
            return View(userInfo);
        }
        public IActionResult login()
        {
            return View();
        }
        // ...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(UserInfo userInfo)
        {
            var hashedPassword = HashPassword(userInfo.PasswordHash);
            var dbUser = await _context.UserInfos
                .FirstOrDefaultAsync(u => u.Email == userInfo.Email && u.PasswordHash == hashedPassword);

            if (dbUser != null)
            {
                HttpContext.Session.SetString("UserEmail", dbUser.Email);
                HttpContext.Session.SetString("UserPassword", hashedPassword);
            }
            ViewBag.Error = "Invalid login credentials";
            return View(userInfo);
        }
        public IActionResult Donations()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "Home");
            }
            var donation = new donation
            {
                FullNames = HttpContext.Session.GetString("UserName") ?? "",
                Email = HttpContext.Session.GetString("UserEmail") ?? ""
            };
            return View(donation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Donations(donation donation)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "Home");
            }

            // Auto-set some fields from session
            if (string.IsNullOrEmpty(donation.FullNames))
            {
                donation.FullNames = HttpContext.Session.GetString("UserName") ?? "";
            }

            if (string.IsNullOrEmpty(donation.Email))
            {
                donation.Email = HttpContext.Session.GetString("UserEmail") ?? "";
            }

           
            donation.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Donations.Add(donation);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "🎉 Thank you for your donation! We will contact you soon.";
                    return RedirectToAction("UserHome", "Home");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "A database error occurred. Please try again.");
                    _logger.LogError(ex, "Database error in LogDonation");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                    _logger.LogError(ex, "Unexpected error in LogDonation");
                }
            }

            return View(donation);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // Add this method inside the HomeController class

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
