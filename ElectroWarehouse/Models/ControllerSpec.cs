using System.ComponentModel.DataAnnotations;

namespace ElectroWarehouse.Models
{
    public class ControllerSpec
    {
        public int Id { get; set; }

        [Display(Name = "Контроллер")]
        public int ControllerDeviceId { get; set; }

        [Display(Name = "Контроллер")]
        public ControllerDevice? ControllerDevice { get; set; }

        [Display(Name = "Электродеталь")]
        public int PartId { get; set; }

        [Display(Name = "Электродеталь")]
        public Part? Part { get; set; }

        [Display(Name = "Количество деталей на изделие")]
        public int QuantityPerUnit { get; set; }
    }
}