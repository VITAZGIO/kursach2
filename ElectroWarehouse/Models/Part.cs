using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Display(Name = "Наименование детали")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Артикул")]
        public string Article { get; set; } = string.Empty;

        [Display(Name = "Количество на складе")]
        public int QuantityInStock { get; set; }

        [Display(Name = "Поставщик")]
        public int SupplierId { get; set; }

        [Display(Name = "Поставщик")]
        public Supplier? Supplier { get; set; }
    }
}