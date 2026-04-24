using lendify.Models;

namespace lendify.Repositories;

public interface IBorrowRepository
{
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task<BorrowRecord?> GetByIdAsync(Guid id);
    Task<BorrowRecord> CreateAsync(BorrowRecord record);
    Task<BorrowRecord> UpdateAsync(BorrowRecord record);
    Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(Guid memberId);
}
