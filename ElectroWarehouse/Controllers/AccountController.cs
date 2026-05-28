using ElectroWarehouse.Data;
using ElectroWarehouse.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectroWarehouse.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var user = _context.AppUsers
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Неверный логин или пароль";
                return View();
            }

            HttpContext.Session.SetString("UserLogin", user.Login);
            HttpContext.Session.SetString("UserRole", user.Role);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AppUser user)
        {
            if (!ModelState.IsValid)
                return View(user);

            if (string.Equals(user.Login, "admin", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Error = "Логин admin зарезервирован";
                return View(user);
            }

            if (_context.AppUsers.Any(u => u.Login == user.Login))
            {
                ViewBag.Error = "Пользователь с таким логином уже существует";
                return View(user);
            }

            user.Role = "User";
            _context.AppUsers.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetString("UserLogin", user.Login);
            HttpContext.Session.SetString("UserRole", user.Role);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
