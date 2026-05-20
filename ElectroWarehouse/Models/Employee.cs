using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите фамилию")]
        [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Отчество не должно превышать 50 символов")]
        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }
    }
}