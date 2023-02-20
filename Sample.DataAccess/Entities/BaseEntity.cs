using System.ComponentModel.DataAnnotations;

namespace Sample.DataAccess.Entities;

public class BaseEntity
{
    public long Id { get; set; }

    public bool IsDeleted { get; set; }

    [MaxLength(128), EmailAddress]
    public string? CreatedBy { get; set; }

    [MaxLength(128), EmailAddress]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; } = DateTime.UtcNow;
}