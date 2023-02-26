using System.ComponentModel.DataAnnotations;

namespace Sample.Business.Dtos.CustomerDtos;

public class CustomerAddDto
{
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        ErrorMessage = "Enter a valid phone number")]
    public string Phone { get; set; } = null!;
}