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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var warehouseOperation = await _context.WarehouseOperations
                .Include(w => w.ControllerDevice)
                .Include(w => w.Employee)
                .Include(w => w.Part)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (warehouseOperation == null) return NotFound();

            return View(warehouseOperation);
        }

        public IActionResult Create()
        {
            FillSelectLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,OperationType,OperationDate,Quantity,EmployeeId,PartId,ControllerDeviceId")]
            WarehouseOperation warehouseOperation)
        {
            if (ModelState.IsValid)
            {
                var result = await ApplyWarehouseOperation(warehouseOperation);

                if (!result.Success)
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                    FillSelectLists(warehouseOperation);
                    return View(warehouseOperation);
                }

                _context.Add(warehouseOperation);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            FillSelectLists(warehouseOperation);
            return View(warehouseOperation);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var warehouseOperation = await _context.WarehouseOperations.FindAsync(id);
            if (warehouseOperation == null) return NotFound();

            FillSelectLists(warehouseOperation);
            return View(warehouseOperation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,OperationType,OperationDate,Quantity,EmployeeId,PartId,ControllerDeviceId")]
            WarehouseOperation warehouseOperation)
        {
            if (id != warehouseOperation.Id) return NotFound();

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
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            FillSelectLists(warehouseOperation);
            return View(warehouseOperation);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var warehouseOperation = await _context.WarehouseOperations
                .Include(w => w.ControllerDevice)
                .Include(w => w.Employee)
                .Include(w => w.Part)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (warehouseOperation == null) return NotFound();

            return View(warehouseOperation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouseOperation = await _context.WarehouseOperations.FindAsync(id);

            if (warehouseOperation != null)
            {
                _context.WarehouseOperations.Remove(warehouseOperation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<(bool Success, string ErrorMessage)> ApplyWarehouseOperation(WarehouseOperation operation)
        {
            if (operation.Quantity <= 0)
            {
                return (false, "Количество должно быть больше нуля.");
            }

            if (operation.OperationType == "Поступление")
            {
                if (!operation.PartId.HasValue)
                    return (false, "Для операции поступления необходимо выбрать электродеталь.");

                var part = await _context.Parts.FindAsync(operation.PartId.Value);
                if (part == null)
                    return (false, "Выбранная электродеталь не найдена.");

                part.QuantityInStock += operation.Quantity;
                return (true, string.Empty);
            }

            if (operation.OperationType == "Сборка")
            {
                if (!operation.ControllerDeviceId.HasValue)
                    return (false, "Для операции сборки необходимо выбрать контроллер.");

                var controller = await _context.ControllerDevices.FindAsync(operation.ControllerDeviceId.Value);
                if (controller == null)
                    return (false, "Выбранный контроллер не найден.");

                var specs = await _context.ControllerSpecs
                    .Include(s => s.Part)
                    .Where(s => s.ControllerDeviceId == operation.ControllerDeviceId.Value)
                    .ToListAsync();

                if (!specs.Any())
                    return (false, "Для выбранного контроллера не задана спецификация.");

                foreach (var spec in specs)
                {
                    if (spec.Part == null)
                        return (false, "В спецификации найдена некорректная электродеталь.");

                    var requiredQuantity = spec.QuantityPerUnit * operation.Quantity;

                    if (spec.Part.QuantityInStock < requiredQuantity)
                    {
                        return (false,
                            $"Недостаточно деталей: {spec.Part.Name}. Требуется {requiredQuantity}, доступно {spec.Part.QuantityInStock}.");
                    }
                }

                foreach (var spec in specs)
                {
                    var requiredQuantity = spec.QuantityPerUnit * operation.Quantity;
                    spec.Part!.QuantityInStock -= requiredQuantity;
                }

                controller.QuantityInStock += operation.Quantity;

                return (true, string.Empty);
            }

            if (operation.OperationType == "Продажа")
            {
                if (!operation.ControllerDeviceId.HasValue)
                    return (false, "Для операции продажи необходимо выбрать контроллер.");

                var controller = await _context.ControllerDevices.FindAsync(operation.ControllerDeviceId.Value);
                if (controller == null)
                    return (false, "Выбранный контроллер не найден.");

                if (controller.QuantityInStock < operation.Quantity)
                    return (false, "Недостаточно контроллеров на складе для продажи.");

                controller.QuantityInStock -= operation.Quantity;
                return (true, string.Empty);
            }

            return (false, "Выбран неизвестный тип операции.");
        }

        private void FillSelectLists(WarehouseOperation? operation = null)
        {
            ViewData["ControllerDeviceId"] = new SelectList(
                _context.ControllerDevices,
                "Id",
                "Name",
                operation?.ControllerDeviceId);

            ViewData["EmployeeId"] = new SelectList(
                _context.Employees.Select(e => new
                {
                    e.Id,
                    FullName = e.LastName + " " + e.FirstName
                }),
                "Id",
                "FullName",
                operation?.EmployeeId);

            ViewData["PartId"] = new SelectList(
                _context.Parts,
                "Id",
                "Name",
                operation?.PartId);
        }

        private bool WarehouseOperationExists(int id)
        {
            return _context.WarehouseOperations.Any(e => e.Id == id);
        }
    }
}