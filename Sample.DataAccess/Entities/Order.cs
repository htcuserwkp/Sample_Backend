using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.DataAccess.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = null!;

    public DateTime OrderPlaced { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ServiceCharge { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    public virtual ICollection<Food> Foods { get; set; } = null!;

    public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
}