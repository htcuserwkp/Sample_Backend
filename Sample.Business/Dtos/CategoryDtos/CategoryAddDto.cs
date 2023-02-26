using System.ComponentModel.DataAnnotations;

namespace Sample.Business.Dtos.CategoryDtos;

public class CategoryAddDto
{
    [Required(ErrorMessage = $"{nameof(Name)} is required")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = $"{nameof(Description)} is required")]
    public string Description { get; set; } = null!;
}