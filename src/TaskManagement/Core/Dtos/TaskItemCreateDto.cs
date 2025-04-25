using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Dtos;

// todo: we can use FluentValidation instead DataAnnotations 
public record TaskItemCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; init; }

    [Required] [StringLength(500)] public string Description { get; init; }
}