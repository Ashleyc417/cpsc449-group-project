using Microsoft.EntityFrameworkCore;
using lendify.Data;
using lendify.Models;

namespace lendify.Repositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly ApplicationDbContext _context;

    public BorrowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
    {
        return await _context.BorrowRecords.ToListAsync();
    }

    public async Task<BorrowRecord?> GetByIdAsync(Guid id)
    {
        return await _context.BorrowRecords.FindAsync(id);
    }

    public async Task<BorrowRecord> CreateAsync(BorrowRecord record)
    {
        record.Id = Guid.NewGuid();
        _context.BorrowRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<BorrowRecord> UpdateAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Update(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(Guid memberId)
    {
        return await _context.BorrowRecords
            .Where(r => r.MemberId == memberId)
            .ToListAsync();
    }
}
