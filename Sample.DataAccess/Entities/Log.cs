using System.ComponentModel.DataAnnotations;

namespace Sample.DataAccess.Entities;

public class Log
{
    public int Id { get; set; }

    [MaxLength(512), Required]
    public string Message { get; set; } = string.Empty;

    [MaxLength(512), Required]
    public string MessageTemplate { get; set; } = string.Empty;

    [MaxLength(128), Required]
    public string Level { get; set; } = string.Empty;

    public DateTime TimeStamp { get; set; }

    [MaxLength(512), Required]
    public string Exception { get; set; } = string.Empty;

    [MaxLength(512), Required]
    public string Properties { get; set; } = string.Empty;

    [MaxLength(512), Required]
    public string LogEvent { get; set; } = string.Empty;
}