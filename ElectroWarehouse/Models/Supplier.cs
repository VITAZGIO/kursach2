using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название поставщика")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        [Display(Name = "Название поставщика")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите город")]
        [StringLength(100, ErrorMessage = "Город не должен превышать 100 символов")]
        [Display(Name = "Город")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите контактную информацию")]
        [StringLength(200, ErrorMessage = "Контактная информация не должна превышать 200 символов")]
        [Display(Name = "Контактная информация")]
        public string ContactInfo { get; set; } = string.Empty;
    }
}