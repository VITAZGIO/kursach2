using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroWarehouse.Models
{
    public class ControllerDevice
    {
        public int Id { get; set; }

        [Display(Name = "Наименование контроллера")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Цена")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Степень защиты IP")]
        public string IpRating { get; set; } = string.Empty;

        [Display(Name = "Количество на складе")]
        public int QuantityInStock { get; set; }
    }
}