namespace ElectroWarehouse.Models
{
    public class ControllerSpec
    {
        public int Id { get; set; }

        public int ControllerDeviceId { get; set; }

        public ControllerDevice? ControllerDevice { get; set; }

        public int PartId { get; set; }

        public Part? Part { get; set; }

        public int QuantityPerUnit { get; set; }
    }
}