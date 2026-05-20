using ElectroWarehouse.Data;
using Microsoft.AspNetCore.Mvc;

namespace ElectroWarehouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.SuppliersCount = _context.Suppliers.Count();
            ViewBag.PartsCount = _context.Parts.Count();
            ViewBag.ControllersCount = _context.ControllerDevices.Count();
            ViewBag.SpecsCount = _context.ControllerSpecs.Count();
            ViewBag.OperationsCount = _context.WarehouseOperations.Count();
            ViewBag.EmployeesCount = _context.Employees.Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}