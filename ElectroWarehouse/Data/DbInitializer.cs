using ElectroWarehouse.Models;

namespace ElectroWarehouse.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            var admin = context.AppUsers.FirstOrDefault(u => u.Login == "admin");
            if (admin == null)
            {
                context.AppUsers.Add(new AppUser
                {
                    Login = "admin",
                    Password = "admin",
                    Role = "Admin"
                });
                context.SaveChanges();
            }
            else
            {
                admin.Role = "Admin";
                context.SaveChanges();
            }

            if (context.Suppliers.Any())
            {
                return;
            }

            var suppliers = new Supplier[]
            {
                new Supplier { Name = "ЭлектроПоставка", ContactInfo = "+7(900)111-11-11, supply@demo.ru" },
                new Supplier { Name = "КомпонентСнаб", ContactInfo = "+7(900)222-22-22, parts@demo.ru" },
                new Supplier { Name = "РадиоМаркет", ContactInfo = "+7(900)333-33-33" }
            };

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            var employees = new Employee[]
            {
                new Employee { LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович" },
                new Employee { LastName = "Петров", FirstName = "Пётр", MiddleName = "Сергеевич" },
                new Employee { LastName = "Сидорова", FirstName = "Анна", MiddleName = "Андреевна" }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();

            var parts = new Part[]
            {
                new Part { Name = "Резистор 10 кОм", Article = "R-10K", QuantityInStock = 120, SupplierId = suppliers[0].Id },
                new Part { Name = "Конденсатор 100 мкФ", Article = "C-100U", QuantityInStock = 75, SupplierId = suppliers[1].Id },
                new Part { Name = "Диод 1N4007", Article = "D-1N4007", QuantityInStock = 200, SupplierId = suppliers[2].Id },
                new Part { Name = "Микроконтроллер STM32", Article = "MCU-STM32", QuantityInStock = 15, SupplierId = suppliers[1].Id },
                new Part { Name = "Печатная плата", Article = "PCB-01", QuantityInStock = 30, SupplierId = suppliers[0].Id }
            };

            context.Parts.AddRange(parts);
            context.SaveChanges();

            var controllers = new ControllerDevice[]
            {
                new ControllerDevice { Name = "Контроллер освещения", Price = 2500, IpRating = "IP54", QuantityInStock = 5 },
                new ControllerDevice { Name = "Контроллер вентиляции", Price = 3200, IpRating = "IP65", QuantityInStock = 2 }
            };

            context.ControllerDevices.AddRange(controllers);
            context.SaveChanges();

            var specs = new ControllerSpec[]
            {
                new ControllerSpec { ControllerDeviceId = controllers[0].Id, PartId = parts[4].Id, QuantityPerUnit = 1 },
                new ControllerSpec { ControllerDeviceId = controllers[0].Id, PartId = parts[3].Id, QuantityPerUnit = 1 },
                new ControllerSpec { ControllerDeviceId = controllers[0].Id, PartId = parts[0].Id, QuantityPerUnit = 4 },
                new ControllerSpec { ControllerDeviceId = controllers[1].Id, PartId = parts[4].Id, QuantityPerUnit = 1 },
                new ControllerSpec { ControllerDeviceId = controllers[1].Id, PartId = parts[0].Id, QuantityPerUnit = 6 }
            };

            context.ControllerSpecs.AddRange(specs);
            context.SaveChanges();

            var operations = new WarehouseOperation[]
            {
                new WarehouseOperation
                {
                    OperationType = "Поступление",
                    OperationDate = DateTime.Now.AddDays(-3),
                    Quantity = 50,
                    EmployeeId = employees[0].Id,
                    PartId = parts[0].Id
                },
                new WarehouseOperation
                {
                    OperationType = "Сборка",
                    OperationDate = DateTime.Now.AddDays(-2),
                    Quantity = 1,
                    EmployeeId = employees[1].Id,
                    ControllerDeviceId = controllers[0].Id
                },
                new WarehouseOperation
                {
                    OperationType = "Продажа",
                    OperationDate = DateTime.Now.AddDays(-1),
                    Quantity = 1,
                    EmployeeId = employees[2].Id,
                    ControllerDeviceId = controllers[0].Id
                }
            };

            context.WarehouseOperations.AddRange(operations);

            context.SaveChanges();
        }
    }
}
