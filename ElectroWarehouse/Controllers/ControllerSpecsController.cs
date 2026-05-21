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
    public class ControllerSpecsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ControllerSpecsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ControllerSpecs
        public async Task<IActionResult> Index(string? search, string? sortOrder)
        {
            ViewBag.Search = search;

            ViewBag.ControllerSort = sortOrder == "controller_asc" ? "controller_desc" : "controller_asc";
            ViewBag.PartSort = sortOrder == "part_asc" ? "part_desc" : "part_asc";
            ViewBag.QuantitySort = sortOrder == "quantity_asc" ? "quantity_desc" : "quantity_asc";

            ViewBag.ControllerIcon = sortOrder == "controller_asc" ? "↑" : "↓";
            ViewBag.PartIcon = sortOrder == "part_asc" ? "↑" : "↓";
            ViewBag.QuantityIcon = sortOrder == "quantity_asc" ? "↑" : "↓";

            var specs = _context.ControllerSpecs
                .Include(c => c.ControllerDevice)
                .Include(c => c.Part)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                specs = specs.Where(c =>
                    c.ControllerDevice!.Name.Contains(search) ||
                    c.Part!.Name.Contains(search) ||
                    c.Part.Article.Contains(search));
            }

            specs = sortOrder switch
            {
                "controller_desc" => specs.OrderByDescending(c => c.ControllerDevice!.Name),
                "part_asc" => specs.OrderBy(c => c.Part!.Name),
                "part_desc" => specs.OrderByDescending(c => c.Part!.Name),
                "quantity_asc" => specs.OrderBy(c => c.QuantityPerUnit),
                "quantity_desc" => specs.OrderByDescending(c => c.QuantityPerUnit),
                _ => specs.OrderBy(c => c.ControllerDevice!.Name)
            };

            return View(await specs.ToListAsync());
        }

        // GET: ControllerSpecs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controllerSpec = await _context.ControllerSpecs
                .Include(c => c.ControllerDevice)
                .Include(c => c.Part)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controllerSpec == null)
            {
                return NotFound();
            }

            return View(controllerSpec);
        }

        // GET: ControllerSpecs/Create
        public IActionResult Create()
        {
            ViewData["ControllerDeviceId"] = new SelectList(_context.ControllerDevices, "Id", "Id");
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id");
            return View();
        }

        // POST: ControllerSpecs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ControllerDeviceId,PartId,QuantityPerUnit")] ControllerSpec controllerSpec)
        {
            if (ModelState.IsValid)
            {
                _context.Add(controllerSpec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ControllerDeviceId"] = new SelectList(_context.ControllerDevices, "Id", "Id", controllerSpec.ControllerDeviceId);
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", controllerSpec.PartId);
            return View(controllerSpec);
        }

        // GET: ControllerSpecs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controllerSpec = await _context.ControllerSpecs.FindAsync(id);
            if (controllerSpec == null)
            {
                return NotFound();
            }
            ViewData["ControllerDeviceId"] = new SelectList(_context.ControllerDevices, "Id", "Id", controllerSpec.ControllerDeviceId);
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", controllerSpec.PartId);
            return View(controllerSpec);
        }

        // POST: ControllerSpecs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ControllerDeviceId,PartId,QuantityPerUnit")] ControllerSpec controllerSpec)
        {
            if (id != controllerSpec.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(controllerSpec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControllerSpecExists(controllerSpec.Id))
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
            ViewData["ControllerDeviceId"] = new SelectList(_context.ControllerDevices, "Id", "Id", controllerSpec.ControllerDeviceId);
            ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", controllerSpec.PartId);
            return View(controllerSpec);
        }

        // GET: ControllerSpecs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controllerSpec = await _context.ControllerSpecs
                .Include(c => c.ControllerDevice)
                .Include(c => c.Part)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controllerSpec == null)
            {
                return NotFound();
            }

            return View(controllerSpec);
        }

        // POST: ControllerSpecs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var controllerSpec = await _context.ControllerSpecs.FindAsync(id);
            if (controllerSpec != null)
            {
                _context.ControllerSpecs.Remove(controllerSpec);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ControllerSpecExists(int id)
        {
            return _context.ControllerSpecs.Any(e => e.Id == id);
        }
    }
}
