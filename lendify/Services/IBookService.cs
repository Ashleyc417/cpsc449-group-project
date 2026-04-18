using lendify.Dtos;

namespace lendify.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto> GetByIdAsync(Guid id);
    Task<BookResponseDto> CreateAsync(BookRequestDto dto);
    Task<BookResponseDto> UpdateAsync(Guid id, BookRequestDto dto);
    Task DeleteAsync(Guid id);
}
