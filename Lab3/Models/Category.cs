namespace Lab3.Models
{
    public class Category
    {
        public long CategoryId { get; set; }
        public required string Name { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
