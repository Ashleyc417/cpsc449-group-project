using System.ComponentModel.DataAnnotations;
using lendify.Models;

namespace lendify.Dtos;

public class BorrowRequestDto
{
    [Required]
    public Guid BookId { get; set; }

    [Required]
    public Guid MemberId { get; set; }
}

public class BorrowResponseDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public BorrowStatus Status { get; set; }
}
