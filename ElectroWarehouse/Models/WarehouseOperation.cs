using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class WarehouseOperation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Выберите тип операции")]
        [Display(Name = "Тип операции")]
        public string OperationType { get; set; } = string.Empty;

        [Display(Name = "Дата операции")]
        public DateTime OperationDate { get; set; } = DateTime.Now;

        [Range(1, 1000000, ErrorMessage = "Количество должно быть больше нуля")]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [Display(Name = "Сотрудник")]
        public int EmployeeId { get; set; }

        [Display(Name = "Сотрудник")]
        public Employee? Employee { get; set; }

        [Display(Name = "Электродеталь")]
        public int? PartId { get; set; }

        [Display(Name = "Электродеталь")]
        public Part? Part { get; set; }

        [Display(Name = "Контроллер")]
        public int? ControllerDeviceId { get; set; }

        [Display(Name = "Контроллер")]
        public ControllerDevice? ControllerDevice { get; set; }
    }
}