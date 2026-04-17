using System.ComponentModel.DataAnnotations;

namespace lendify.Dtos;

public class BookRequestDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    [Required]
    public string Isbn { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "TotalCopies must be greater than 0")]
    public int TotalCopies { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "AvailableCopies must be 0 or greater")]
    public int AvailableCopies { get; set; }
}

public class BookResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}
