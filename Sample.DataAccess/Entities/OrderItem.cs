using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.DataAccess.Entities
{
    public class OrderItem : BaseEntity
    {
        public long FoodId { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public long OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
    }
}
