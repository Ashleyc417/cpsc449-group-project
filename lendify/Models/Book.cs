using System.ComponentModel.DataAnnotations;

namespace lendify.Models;

public class Book
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Author { get; set; } = string.Empty;
	public string Isbn { get; set; } = string.Empty;
	public int TotalCopies { get; set; } = 0;
	[ConcurrencyCheck]
	public int AvailableCopies { get; set; } = 0;
}
