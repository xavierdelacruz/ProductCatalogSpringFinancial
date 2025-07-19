namespace ProductCatalog.Domain
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; } 
        public int StockQuantity { get; private set; }  
        public string Brand { get; private set; }
        public string Category { get; private set; }
        public string SKU { get; private set; }
        public string AvaiabilityStatus { get; private set; }
        public double CustomerRating { get; private set; }
        public Product(int id, string name, string description, decimal price, int stockQuantity, 
                       string brand, string category, string sku, string availabilityStatus, double customerRating)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            Brand = brand;
            Category = category;
            SKU = sku;
            AvaiabilityStatus = availabilityStatus;
            CustomerRating = customerRating;
        }
    }
}
