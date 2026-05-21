using ElectroWarehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectroWarehouse.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ControllerDevice> ControllerDevices { get; set; }
        public DbSet<ControllerSpec> ControllerSpecs { get; set; }
        public DbSet<WarehouseOperation> WarehouseOperations { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}