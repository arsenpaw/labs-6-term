using Lab2.DTOs;
using Lab2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;

namespace Lab2.Controllers;

[ApiController]
[Route("api/games")]
[Produces("application/json")]
public class GamesController(IGameService gameService) : ControllerBase
{
    private static readonly string HtmlTemplate = LoadTemplate();

    private static string LoadTemplate()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "Lab2.Templates.games.html";
        using var stream = assembly.GetManifestResourceStream(resourceName)
                           ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() => Ok(await gameService.GetAllGamesAsync());

    [HttpGet("ui")]
    [Produces("text/html")]
    public async Task<ContentResult> GetAllUi()
    {
        var games = (await gameService.GetAllGamesAsync()).ToList();

        string content;
        if (games.Count == 0)
        {
            content = "<div class=\"empty\">No games found.</div>";
        }
        else
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"grid\">");
            foreach (var g in games)
            {
                var title = System.Web.HttpUtility.HtmlEncode(g.Title);
                var desc  = System.Web.HttpUtility.HtmlEncode(g.Description ?? "No description available.");
                var statusBadge = g.IsActive
                    ? "<span class=\"badge badge-active\">✔ Active</span>"
                    : "<span class=\"badge badge-inactive\">✖ Inactive</span>";

                sb.AppendLine($"""
                    <div class="card">
                        <div class="card-title">{title}</div>
                        <div class="card-desc">{desc}</div>
                        <div class="card-meta">
                            {statusBadge}
                    """);

                if (!string.IsNullOrWhiteSpace(g.Genre))
                    sb.AppendLine($"<span class=\"badge badge-genre\">🎭 {System.Web.HttpUtility.HtmlEncode(g.Genre)}</span>");
                if (!string.IsNullOrWhiteSpace(g.Publisher))
                    sb.AppendLine($"<span class=\"badge badge-pub\">🏢 {System.Web.HttpUtility.HtmlEncode(g.Publisher)}</span>");
                if (g.Price.HasValue)
                    sb.AppendLine($"<span class=\"badge badge-price\">💲 {g.Price.Value:F2}</span>");
                if (g.Tags is { Length: > 0 })
                    foreach (var tag in g.Tags)
                        sb.AppendLine($"<span class=\"badge badge-tag\">#{System.Web.HttpUtility.HtmlEncode(tag)}</span>");

                sb.AppendLine($"""
                        </div>
                        <div class="card-id">{g.Id}</div>
                    </div>
                    """);
            }
            sb.AppendLine("</div>");
            content = sb.ToString();
        }

        var html = HtmlTemplate.Replace("{{CONTENT}}", content);
        return Content(html, "text/html");
    }

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