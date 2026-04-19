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
    public Guid BookId { get; set; } // Guid for book id? I Might be wrong
    public Guid MemberId { get; set; } // same here
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public BorrowStatus Status { get; set; } = BorrowStatus.Returned;
}
