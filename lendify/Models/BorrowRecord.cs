namespace lendify.Models;

// Could probably just be a bool for optimising
// if just two enum values
public enum BorrowStatus
{
    Borrowed,
    Returned
}

public class BorrowRecord
{
	public Guid Id { get; set; }
	public Guid BookId { get; set; }
	public Guid MemberId { get; set; }
	public DateTime BorrowDate { get; set; }
	public DateTime? ReturnDate { get; set; }
	public BorrowStatus Status { get; set; }
}
