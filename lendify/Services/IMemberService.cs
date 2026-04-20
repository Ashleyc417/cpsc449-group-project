using lendify.Dtos;

namespace lendify.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberResponseDto>> GetAllAsync();
    Task<MemberResponseDto> GetByIdAsync(Guid id);
    Task<MemberResponseDto> CreateAsync(MemberRequestDto dto);
    Task<MemberResponseDto> UpdateAsync(Guid id, MemberRequestDto dto);
    Task DeleteAsync(Guid id);
}
