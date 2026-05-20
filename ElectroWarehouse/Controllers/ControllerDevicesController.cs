using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectroWarehouse.Data;
using ElectroWarehouse.Models;

namespace ElectroWarehouse.Controllers
{
    public class ControllerDevicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ControllerDevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ControllerDevices
        public async Task<IActionResult> Index()
        {
            return View(await _context.ControllerDevices.ToListAsync());
        }

        // GET: ControllerDevices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controllerDevice = await _context.ControllerDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controllerDevice == null)
            {
                return NotFound();
            }

            return View(controllerDevice);
        }

        // GET: ControllerDevices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ControllerDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,IpRating,QuantityInStock")] ControllerDevice controllerDevice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(controllerDevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(controllerDevice);
        }

        // GET: ControllerDevices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controllerDevice = await _context.ControllerDevices.FindAsync(id);
            if (controllerDevice == null)
            {
                return NotFound();
            }
            return View(controllerDevice);
        }

        // POST: ControllerDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,IpRating,QuantityInStock")] ControllerDevice controllerDevice)
        {
            if (id != controllerDevice.Id)
            {
                return NotFound();
            }

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
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(controllerDevice);
        }

        // GET: ControllerDevices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controllerDevice = await _context.ControllerDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controllerDevice == null)
            {
                return NotFound();
            }

            return View(controllerDevice);
        }

        // POST: ControllerDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var controllerDevice = await _context.ControllerDevices.FindAsync(id);
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
