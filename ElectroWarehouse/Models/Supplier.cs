using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Display(Name = "Название поставщика")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Контактная информация")]
        public string ContactInfo { get; set; } = string.Empty;
    }
}