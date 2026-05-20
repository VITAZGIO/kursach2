namespace ElectroWarehouse.Models
{
    public class Part
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Article { get; set; } = string.Empty;

        public int QuantityInStock { get; set; }

        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }
    }
}