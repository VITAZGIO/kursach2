using ElectroWarehouse.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectroWarehouse.Controllers
{
    public class HomeController : BaseController
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

            ViewBag.LowStockParts = _context.Parts
                .Where(p => p.QuantityInStock < 20)
                .OrderBy(p => p.QuantityInStock)
                .ToList();

            ViewBag.AllParts = _context.Parts
                .Include(p => p.Supplier)
                .OrderBy(p => p.Name)
                .ToList();

            ViewBag.AllControllers = _context.ControllerDevices
                .OrderBy(c => c.Name)
                .ToList();

            return View();
        }
    }
}