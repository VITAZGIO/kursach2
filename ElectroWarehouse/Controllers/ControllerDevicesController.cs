using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectroWarehouse.Data;
using ElectroWarehouse.Models;

namespace ElectroWarehouse.Controllers
{
    public class ControllerDevicesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ControllerDevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, string? sortOrder)
        {
            ViewBag.Search = search;

            ViewBag.NameSort =
                sortOrder == "name" ? "name_desc" : "name";

            ViewBag.PriceSort =
                sortOrder == "price" ? "price_desc" : "price";

            ViewBag.QuantitySort =
                sortOrder == "quantity" ? "quantity_desc" : "quantity";

            var controllers = _context.ControllerDevices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                controllers = controllers.Where(c =>
                    c.Name.Contains(search) ||
                    c.IpRating.Contains(search));
            }

            controllers = sortOrder switch
            {
                "name" => controllers.OrderBy(c => c.Name),
                "name_desc" => controllers.OrderByDescending(c => c.Name),

                "price" => controllers.OrderBy(c => c.Price),
                "price_desc" => controllers.OrderByDescending(c => c.Price),

                "quantity" => controllers.OrderBy(c => c.QuantityInStock),
                "quantity_desc" => controllers.OrderByDescending(c => c.QuantityInStock),

                _ => controllers.OrderBy(c => c.Name)
            };

            return View(await controllers.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var controllerDevice = await _context.ControllerDevices
                .FirstOrDefaultAsync(m => m.Id == id);

            if (controllerDevice == null)
                return NotFound();

            return View(controllerDevice);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Price,IpRating,QuantityInStock")]
            ControllerDevice controllerDevice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(controllerDevice);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(controllerDevice);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var controllerDevice =
                await _context.ControllerDevices.FindAsync(id);

            if (controllerDevice == null)
                return NotFound();

            return View(controllerDevice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Price,IpRating,QuantityInStock")]
            ControllerDevice controllerDevice)
        {
            if (id != controllerDevice.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(controllerDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControllerDeviceExists(controllerDevice.Id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(controllerDevice);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var controllerDevice = await _context.ControllerDevices
                .FirstOrDefaultAsync(m => m.Id == id);

            if (controllerDevice == null)
                return NotFound();

            return View(controllerDevice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var controllerDevice =
                await _context.ControllerDevices.FindAsync(id);

            if (controllerDevice != null)
            {
                _context.ControllerDevices.Remove(controllerDevice);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ControllerDeviceExists(int id)
        {
            return _context.ControllerDevices.Any(e => e.Id == id);
        }
    }
}