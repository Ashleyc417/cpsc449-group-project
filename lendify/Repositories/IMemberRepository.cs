using lendify.Models;

namespace lendify.Repositories;

public interface IMemberRepository
{
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(Guid id);
    Task<Member> CreateAsync(Member member);
    Task<Member> UpdateAsync(Member member);
    Task DeleteAsync(Member member);
    Task<bool> ExistsByEmailAsync(string email);
}
