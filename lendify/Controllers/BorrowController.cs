using Microsoft.AspNetCore.Mvc;
using lendify.Dtos;
using lendify.Services;

namespace lendify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowController : ControllerBase
{
    private readonly IBorrowService _service;

    public BorrowController(IBorrowService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _service.GetAllAsync();
        return Ok(records);
    }

    [HttpGet("member/{memberId}")]
    public async Task<IActionResult> GetByMember(Guid memberId)
    {
        var records = await _service.GetByMemberIdAsync(memberId);
        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Borrow([FromBody] BorrowRequestDto dto)
    {
        var record = await _service.BorrowBookAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = record.Id }, record);
    }

    [HttpPut("{id}/return")]
    public async Task<IActionResult> Return(Guid id)
    {
        var record = await _service.ReturnBookAsync(id);
        return Ok(record);
    }
}
