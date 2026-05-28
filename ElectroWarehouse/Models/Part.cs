using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите наименование детали")]
        [StringLength(100, ErrorMessage = "Наименование не должно превышать 100 символов")]
        [Display(Name = "Наименование детали")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите артикул")]
        [StringLength(50, ErrorMessage = "Артикул не должен превышать 50 символов")]
        [Display(Name = "Артикул")]
        public string Article { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "Количество не может быть отрицательным")]
        [Display(Name = "Количество на складе")]
        public int QuantityInStock { get; set; }

        [Display(Name = "Поставщик")]
        public int SupplierId { get; set; }

        [Display(Name = "Поставщик")]
        public Supplier? Supplier { get; set; }

        [Display(Name = "Фото")]
        public string? ImagePath { get; set; }
    }
}
