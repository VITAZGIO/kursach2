using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectroWarehouse.Data;
using ElectroWarehouse.Models;

namespace ElectroWarehouse.Controllers
{
    public class PartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, string? sortOrder)
        {
            ViewBag.Search = search;
            ViewBag.NameSort = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.QuantitySort = sortOrder == "quantity" ? "quantity_desc" : "quantity";

            var parts = _context.Parts
                .Include(p => p.Supplier)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                parts = parts.Where(p =>
                    p.Name.Contains(search) ||
                    p.Article.Contains(search) ||
                    p.Supplier!.Name.Contains(search));
            }

            parts = sortOrder switch
            {
                "name" => parts.OrderBy(p => p.Name),
                "name_desc" => parts.OrderByDescending(p => p.Name),
                "quantity" => parts.OrderBy(p => p.QuantityInStock),
                "quantity_desc" => parts.OrderByDescending(p => p.QuantityInStock),
                _ => parts.OrderBy(p => p.Name)
            };

            return View(await parts.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var part = await _context.Parts
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (part == null) return NotFound();

            return View(part);
        }

        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Article,QuantityInStock,SupplierId")] Part part)
        {
            if (ModelState.IsValid)
            {
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", part.SupplierId);
            return View(part);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var part = await _context.Parts.FindAsync(id);
            if (part == null) return NotFound();

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", part.SupplierId);
            return View(part);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Article,QuantityInStock,SupplierId")] Part part)
        {
            if (id != part.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.Id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", part.SupplierId);
            return View(part);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var part = await _context.Parts
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (part == null) return NotFound();

            return View(part);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _context.Parts.FindAsync(id);

            if (part != null)
            {
                _context.Parts.Remove(part);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.Id == id);
        }
    }
}