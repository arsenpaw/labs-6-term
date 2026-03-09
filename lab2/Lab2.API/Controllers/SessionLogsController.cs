using Lab2.DTOs;
using Lab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers;

[ApiController]
[Route("api/logs")]
[Produces("application/json")]
public class SessionLogsController(ISessionLogService logService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SessionLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() => Ok(await logService.GetAllAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SessionLogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var log = await logService.GetByIdAsync(id);
        return log is null ? NotFound() : Ok(log);
    }

    [HttpGet("by-session/{sessionId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<SessionLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySession(Guid sessionId)
        => Ok(await logService.GetBySessionIdAsync(sessionId));

    [HttpPost]
    [ProducesResponseType(typeof(SessionLogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateSessionLogDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var created = await logService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SessionLogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSessionLogDto dto)
    {
        var result = await logService.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await logService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
