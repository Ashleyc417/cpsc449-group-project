using Microsoft.Extensions.Caching.Memory;
using lendify.Dtos;
using lendify.Models;
using lendify.Repositories;

namespace lendify.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repo;
    private readonly IMemoryCache _cache;
    private const string AllBooksCacheKey = "all_books";

    public BookService(IBookRepository repo, IMemoryCache cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
    {
        if (_cache.TryGetValue(AllBooksCacheKey, out IEnumerable<BookResponseDto>? cached) && cached is not null)
            return cached;

        var books = await _repo.GetAllAsync();
        var result = books.Select(ToDto).ToList();
        _cache.Set(AllBooksCacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    public async Task<BookResponseDto> GetByIdAsync(Guid id)
    {
        var cacheKey = $"book_{id}";
        if (_cache.TryGetValue(cacheKey, out BookResponseDto? cached) && cached is not null)
            return cached;

        var book = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Book with id {id} not found.");

        var result = ToDto(book);
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    public async Task<BookResponseDto> CreateAsync(BookRequestDto dto)
    {
        if (dto.AvailableCopies > dto.TotalCopies)
            throw new InvalidOperationException("AvailableCopies cannot exceed TotalCopies.");

        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            Isbn = dto.Isbn,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.AvailableCopies
        };

        var created = await _repo.CreateAsync(book);
        _cache.Remove(AllBooksCacheKey);
        return ToDto(created);
    }

    public async Task<BookResponseDto> UpdateAsync(Guid id, BookRequestDto dto)
    {
        var book = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Book with id {id} not found.");

        if (dto.AvailableCopies > dto.TotalCopies)
            throw new InvalidOperationException("AvailableCopies cannot exceed TotalCopies.");

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.Isbn = dto.Isbn;
        book.TotalCopies = dto.TotalCopies;
        book.AvailableCopies = dto.AvailableCopies;

        var updated = await _repo.UpdateAsync(book);
        _cache.Remove(AllBooksCacheKey);
        _cache.Remove($"book_{id}");
        return ToDto(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Book with id {id} not found.");

        await _repo.DeleteAsync(book);
        _cache.Remove(AllBooksCacheKey);
        _cache.Remove($"book_{id}");
    }

    private static BookResponseDto ToDto(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Isbn = book.Isbn,
        TotalCopies = book.TotalCopies,
        AvailableCopies = book.AvailableCopies
    };
}
