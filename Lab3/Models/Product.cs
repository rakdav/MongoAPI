using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lab3.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        public required string Name { get; set; }
        [Column(TypeName ="decimal(8,2)")]
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        public long SupplierId { get; set; }
        [JsonIgnore]
        public Supplier? Supplier { get; set; }

    }
}
