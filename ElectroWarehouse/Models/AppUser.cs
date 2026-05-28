using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Роль")]
        public string Role { get; set; } = "User";
    }
}
