using Lab2.DTOs;
using Lab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers;

[ApiController]
[Route("api/sessions")]
[Produces("application/json")]
public class GameSessionsController(IGameSessionService sessionService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameSessionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
        => Ok(await sessionService.GetAllAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GameSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var session = await sessionService.GetByIdAsync(id);
        return session is null ? NotFound() : Ok(session);
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<GameSessionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId)
        => Ok(await sessionService.GetByUserIdAsync(userId));

    [HttpGet("by-game/{gameId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<GameSessionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByGame(Guid gameId)
        => Ok(await sessionService.GetByGameIdAsync(gameId));

    [HttpPost]
    [ProducesResponseType(typeof(GameSessionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateGameSessionDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var created = await sessionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GameSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGameSessionDto dto)
    {
        var result = await sessionService.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await sessionService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
