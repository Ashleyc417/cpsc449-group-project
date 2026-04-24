using lendify.Dtos;

namespace lendify.Services;

public interface IBorrowService
{
    Task<IEnumerable<BorrowResponseDto>> GetAllAsync();
    Task<IEnumerable<BorrowResponseDto>> GetByMemberIdAsync(Guid memberId);
    Task<BorrowResponseDto> BorrowBookAsync(BorrowRequestDto dto);
    Task<BorrowResponseDto> ReturnBookAsync(Guid borrowRecordId);
}
