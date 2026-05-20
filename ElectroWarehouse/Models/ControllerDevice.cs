using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroWarehouse.Models
{
    public class ControllerDevice
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string IpRating { get; set; } = string.Empty;

        public int QuantityInStock { get; set; }
    }
}