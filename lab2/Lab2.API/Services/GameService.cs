using Lab2.Abstractions;
using Lab2.DTOs;
using Lab2.Models;

namespace Lab2.Services;

public class GameService(IUnitOfWork uow) : IGameService
{
    public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
    {
        var games = await uow.Games.GetAllAsync();
        return games.Select(ToDto);
    }

    public async Task<GameDto?> GetGameByIdAsync(Guid id)
    {
        var game = await uow.Games.GetByIdAsync(id);
        return game is null ? null : ToDto(game);
    }

    public async Task<GameDto> CreateGameAsync(CreateGameDto dto)
    {
        if (dto.Embedding is not null && dto.Embedding.Length != 768)
            throw new ArgumentException("Embedding must be a vector of exactly 768 floats.");

        var game = new Game
        {
            Title = dto.Title,
            Description = dto.Description,
            Genre = dto.Genre,
            Price = dto.Price,
            Publisher = dto.Publisher,
            Embedding = dto.Embedding,
            Tags = dto.Tags,
            IsActive = dto.IsActive
        };
        await uow.Games.AddAsync(game);
        await uow.SaveChangesAsync();
        return ToDto(game);
    }

    public async Task<GameDto?> UpdateGameAsync(Guid id, UpdateGameDto dto)
    {
        if (dto.Embedding is not null && dto.Embedding.Length != 768)
            throw new ArgumentException("Embedding must be a vector of exactly 768 floats.");

        var existing = await uow.Games.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.Genre = dto.Genre;
        existing.Price = dto.Price;
        existing.Publisher = dto.Publisher;
        existing.Embedding = dto.Embedding;
        existing.Tags = dto.Tags;
        existing.IsActive = dto.IsActive;

        var result = await uow.Games.UpdateAsync(id, existing);
        if (result is null) return null;
        await uow.SaveChangesAsync();
        return ToDto(result);
    }

    public async Task<bool> DeleteGameAsync(Guid id)
    {
        var deleted = await uow.Games.DeleteAsync(id);
        if (deleted) await uow.SaveChangesAsync();
        return deleted;
    }

    private static GameDto ToDto(Game g) => new()
    {
        Id = g.Id,
        Title = g.Title,
        Description = g.Description,
        Genre = g.Genre,
        Price = g.Price,
        Publisher = g.Publisher,
        Embedding = g.Embedding,
        Tags = g.Tags,
        IsActive = g.IsActive
    };
}

