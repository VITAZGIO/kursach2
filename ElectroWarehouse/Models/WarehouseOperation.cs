namespace ElectroWarehouse.Models
{
    public class WarehouseOperation
    {
        public int Id { get; set; }

        public string OperationType { get; set; } = string.Empty;

        public DateTime OperationDate { get; set; }

        public int Quantity { get; set; }

        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        public int? PartId { get; set; }

        public Part? Part { get; set; }

        public int? ControllerDeviceId { get; set; }

        public ControllerDevice? ControllerDevice { get; set; }
    }
}