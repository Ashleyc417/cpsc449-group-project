using lendify.Dtos;
using lendify.Models;
using lendify.Repositories;

namespace lendify.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repo;

    public MemberService(IMemberRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<MemberResponseDto>> GetAllAsync()
    {
        var members = await _repo.GetAllAsync();
        return members.Select(ToDto).ToList();
    }

    public async Task<MemberResponseDto> GetByIdAsync(Guid id)
    {
        var member = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Member with id {id} not found.");
        return ToDto(member);
    }

    public async Task<MemberResponseDto> CreateAsync(MemberRequestDto dto)
    {
        if (await _repo.ExistsByEmailAsync(dto.Email))
            throw new ArgumentException($"A member with email {dto.Email} already exists.");

        var member = new Member
        {
            FullName = dto.FullName,
            Email = dto.Email,
            MembershipDate = DateTime.UtcNow
        };

        var created = await _repo.CreateAsync(member);
        return ToDto(created);
    }

    public async Task<MemberResponseDto> UpdateAsync(Guid id, MemberRequestDto dto)
    {
        var member = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Member with id {id} not found.");

        // if email changed, make sure new email isnt taken
        if (!member.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)
            && await _repo.ExistsByEmailAsync(dto.Email))
            throw new ArgumentException($"A member with email {dto.Email} already exists.");

        member.FullName = dto.FullName;
        member.Email = dto.Email;

        var updated = await _repo.UpdateAsync(member);
        return ToDto(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var member = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Member with id {id} not found.");
        await _repo.DeleteAsync(member);
    }

    private static MemberResponseDto ToDto(Member member) => new()
    {
        Id = member.Id,
        FullName = member.FullName,
        Email = member.Email,
        MembershipDate = member.MembershipDate
    };
}
