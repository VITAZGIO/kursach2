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
    public class WarehouseOperationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehouseOperationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WarehouseOperations
        public async Task<IActionResult> Index(string? search)
        {
            var operations = _context.WarehouseOperations
                .Include(w => w.Employee)
                .Include(w => w.Part)
                .Include(w => w.ControllerDevice)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                operations = operations.Where(w =>
                    w.OperationType.Contains(search) ||
                    w.Employee!.FirstName.Contains(search) ||
                    w.Employee.LastName.Contains(search) ||
                    (w.Part != null && w.Part.Name.Contains(search)) ||
                    (w.ControllerDevice != null && w.ControllerDevice.Name.Contains(search)));
            }

            ViewBag.Search = search;

            return View(await operations.ToListAsync());
        }

        // GET: WarehouseOperations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseOperation = await _context.WarehouseOperations
                .Include(w => w.ControllerDevice)
                .Include(w => w.Employee)
                .Include(w => w.Part)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouseOperation == null)
            {
                return NotFound();
            }

            return View(warehouseOperation);
        }

        // GET: WarehouseOperations/Create
        public IActionResult Create()
        {
            ViewData["ControllerDeviceId"] = new SelectList(_context.ControllerDevices, "Id", "Id");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id");
            return View();
        }

        // POST: WarehouseOperations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OperationType,OperationDate,Quantity,EmployeeId,PartId,ControllerDeviceId")] WarehouseOperation warehouseOperation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warehouseOperation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ControllerDeviceId"] = new SelectList(_context.ControllerDevices, "Id", "Id", warehouseOperation.ControllerDeviceId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", warehouseOperation.EmployeeId);
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", warehouseOperation.PartId);
            return View(warehouseOperation);
        }

        // GET: WarehouseOperations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseOperation = await _context.WarehouseOperations.FindAsync(id);
            if (warehouseOperation == null)
            {
                return NotFound();
            }
            ViewData["ControllerDeviceId"] = new SelectList(_context.ControllerDevices, "Id", "Id", warehouseOperation.ControllerDeviceId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", warehouseOperation.EmployeeId);
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", warehouseOperation.PartId);
            return View(warehouseOperation);
        }

        // POST: WarehouseOperations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OperationType,OperationDate,Quantity,EmployeeId,PartId,ControllerDeviceId")] WarehouseOperation warehouseOperation)
        {
            if (id != warehouseOperation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouseOperation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseOperationExists(warehouseOperation.Id))
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
            ViewData["ControllerDeviceId"] = new SelectList(_context.ControllerDevices, "Id", "Id", warehouseOperation.ControllerDeviceId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", warehouseOperation.EmployeeId);
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", warehouseOperation.PartId);
            return View(warehouseOperation);
        }

        // GET: WarehouseOperations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseOperation = await _context.WarehouseOperations
                .Include(w => w.ControllerDevice)
                .Include(w => w.Employee)
                .Include(w => w.Part)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouseOperation == null)
            {
                return NotFound();
            }

            return View(warehouseOperation);
        }

        // POST: WarehouseOperations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouseOperation = await _context.WarehouseOperations.FindAsync(id);
            if (warehouseOperation != null)
            {
                _context.WarehouseOperations.Remove(warehouseOperation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseOperationExists(int id)
        {
            return _context.WarehouseOperations.Any(e => e.Id == id);
        }
    }
}
