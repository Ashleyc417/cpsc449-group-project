namespace lendify.Models;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Isbn { get; set; } = 0;
    public int TotalCopies { get; set; } = 0;
    public int AvailableCopies { get; set; } = 0;
}
