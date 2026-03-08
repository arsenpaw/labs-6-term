using Lab2.DTOs;
using Lab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers;

[ApiController]
[Route("api/games")]
[Produces("application/json")]
public class GamesController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() => Ok(await gameService.GetAllGamesAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var game = await gameService.GetGameByIdAsync(id);
        return game is null ? NotFound() : Ok(game);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateGameDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var created = await gameService.CreateGameAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGameDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await gameService.UpdateGameAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await gameService.DeleteGameAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}