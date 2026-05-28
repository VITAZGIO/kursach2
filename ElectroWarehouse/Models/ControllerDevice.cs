using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroWarehouse.Models
{
    public class ControllerDevice
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите наименование контроллера")]
        [StringLength(100, ErrorMessage = "Наименование не должно превышать 100 символов")]
        [Display(Name = "Наименование контроллера")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 10000000, ErrorMessage = "Цена не может быть отрицательной")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Введите степень защиты IP")]
        [StringLength(20, ErrorMessage = "Степень защиты не должна превышать 20 символов")]
        [Display(Name = "Степень защиты IP")]
        public string IpRating { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "Количество не может быть отрицательным")]
        [Display(Name = "Количество на складе")]
        public int QuantityInStock { get; set; }

        [Display(Name = "Фото")]
        public string? ImagePath { get; set; }
    }
}
