using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }
    }
}