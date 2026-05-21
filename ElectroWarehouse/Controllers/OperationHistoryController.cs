using ElectroWarehouse.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectroWarehouse.Controllers
{
    public class OperationHistoryController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public OperationHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var operations = await _context.WarehouseOperations
                .Include(o => o.Employee)
                .Include(o => o.Part)
                .Include(o => o.ControllerDevice)
                .OrderByDescending(o => o.OperationDate)
                .ThenByDescending(o => o.Id)
                .Take(10)
                .ToListAsync();

            return View(operations);
        }
    }
}