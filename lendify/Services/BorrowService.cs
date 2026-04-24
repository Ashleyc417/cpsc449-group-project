using Microsoft.EntityFrameworkCore;
using lendify.Data;
using lendify.Dtos;
using lendify.Models;
using lendify.Repositories;

namespace lendify.Services;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepo;
    private readonly IBookRepository _bookRepo;
    private readonly IMemberRepository _memberRepo;
    private readonly ApplicationDbContext _context;

    // used to prevent race conditions when borrowing the last copy
    private static readonly SemaphoreSlim _borrowLock = new(1, 1);

    public BorrowService(
        IBorrowRepository borrowRepo,
        IBookRepository bookRepo,
        IMemberRepository memberRepo,
        ApplicationDbContext context)
    {
        _borrowRepo = borrowRepo;
        _bookRepo = bookRepo;
        _memberRepo = memberRepo;
        _context = context;
    }

    public async Task<IEnumerable<BorrowResponseDto>> GetAllAsync()
    {
        var records = await _borrowRepo.GetAllAsync();
        return records.Select(ToDto).ToList();
    }

    public async Task<IEnumerable<BorrowResponseDto>> GetByMemberIdAsync(Guid memberId)
    {
        var records = await _borrowRepo.GetByMemberIdAsync(memberId);
        return records.Select(ToDto).ToList();
    }

    public async Task<BorrowResponseDto> BorrowBookAsync(BorrowRequestDto dto)
    {
        await _borrowLock.WaitAsync();
        try
        {
            var book = await _bookRepo.GetByIdAsync(dto.BookId)
                ?? throw new KeyNotFoundException($"Book with id {dto.BookId} not found.");

            var member = await _memberRepo.GetByIdAsync(dto.MemberId)
                ?? throw new KeyNotFoundException($"Member with id {dto.MemberId} not found.");

            if (book.AvailableCopies <= 0)
                throw new InvalidOperationException("No available copies of this book.");

            book.AvailableCopies--;

            var record = new BorrowRecord
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                BorrowDate = DateTime.UtcNow,
                Status = BorrowStatus.Borrowed
            };

            try
            {
                await _bookRepo.UpdateAsync(book);
                var created = await _borrowRepo.CreateAsync(record);
                return ToDto(created);
            }
            catch (DbUpdateConcurrencyException)
            {
                // another request updated AvailableCopies at the same time
                throw new InvalidOperationException("Could not borrow book due to a conflict, please try again.");
            }
        }
        finally
        {
            _borrowLock.Release();
        }
    }

    public async Task<BorrowResponseDto> ReturnBookAsync(Guid borrowRecordId)
    {
        var record = await _borrowRepo.GetByIdAsync(borrowRecordId)
            ?? throw new KeyNotFoundException($"Borrow record with id {borrowRecordId} not found.");

        if (record.Status == BorrowStatus.Returned)
            throw new InvalidOperationException("This book has already been returned.");

        var book = await _bookRepo.GetByIdAsync(record.BookId)
            ?? throw new KeyNotFoundException($"Book with id {record.BookId} not found.");

        book.AvailableCopies++;
        record.Status = BorrowStatus.Returned;
        record.ReturnDate = DateTime.UtcNow;

        await _bookRepo.UpdateAsync(book);
        var updated = await _borrowRepo.UpdateAsync(record);
        return ToDto(updated);
    }

    private static BorrowResponseDto ToDto(BorrowRecord record) => new()
    {
        Id = record.Id,
        BookId = record.BookId,
        MemberId = record.MemberId,
        BorrowDate = record.BorrowDate,
        ReturnDate = record.ReturnDate,
        Status = record.Status
    };
}
