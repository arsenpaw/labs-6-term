using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using DotNetEnv;
using Lab2.Data;
using Lab2.Models;
using Microsoft.EntityFrameworkCore;

var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
if (!File.Exists(envPath))
    envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

Env.Load(envPath);

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                       ?? throw new InvalidOperationException("CONNECTION_STRING not found in .env");

Console.WriteLine($"Connecting to: {connectionString}");

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(connectionString)
    .Options;

await using var db = new AppDbContext(options);
await db.Database.MigrateAsync();

static string ResolveCsv(string fileName)
{
    var binPath = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
    if (File.Exists(binPath)) return binPath;
    var cwdPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);
    if (File.Exists(cwdPath)) return cwdPath;
    throw new FileNotFoundException($"CSV file not found: {fileName}");
}

static CsvReader OpenCsv(string path)
{
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        MissingFieldFound = null
    };
    return new CsvReader(new StreamReader(path), config);
}

var users = new List<User>();
var games = new List<Game>();
var sessions = new List<GameSession>();
var logs = new List<SessionLog>();

using (var csv = OpenCsv(ResolveCsv("data.csv")))
{
    await csv.ReadAsync();
    csv.ReadHeader();

    while (await csv.ReadAsync())
    {
        var type = csv.GetField<string>("Type");

        switch (type)
        {
            case "User":
            {
                var rawPass = csv.GetField<string>("Password")!;
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Username = csv.GetField<string>("Username")!,
                    Email = csv.GetField<string>("Email")!,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(rawPass),
                    RegionCode = csv.GetField<string>("RegionCode"),
                    IsVerified = csv.GetField<bool>("IsVerified"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                break;
            }
            case "Game":
            {
                var tagsRaw = csv.GetField<string>("Tags") ?? "";
                var tags = tagsRaw.Length > 0
                    ? tagsRaw.Split('|', StringSplitOptions.RemoveEmptyEntries)
                    : [];
                var priceRaw = csv.GetField<string>("Price");
                games.Add(new Game
                {
                    Id = Guid.NewGuid(),
                    Title = csv.GetField<string>("Title")!,
                    Description = csv.GetField<string>("Description"),
                    Genre = csv.GetField<string>("Genre"),
                    Price = priceRaw is { Length: > 0 } ? decimal.Parse(priceRaw, CultureInfo.InvariantCulture) : null,
                    Publisher = csv.GetField<string>("Publisher"),
                    IsActive = csv.GetField<bool>("IsActive"),
                    Tags = tags,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                break;
            }
            case "Session":
            {
                var userIdx = csv.GetField<int>("UserIndex");
                if (userIdx == 49)
                    Console.WriteLine("asf");
                var gameIdx = csv.GetField<int>("GameIndex");
                var startOff = csv.GetField<double>("StartedAtOffsetHours");
                var endOffRaw = csv.GetField<string>("EndedAtOffsetHours");
                DateTime? endedAt = endOffRaw is { Length: > 0 }
                    ? DateTime.UtcNow.AddHours(double.Parse(endOffRaw, CultureInfo.InvariantCulture))
                    : null;
                var Status = csv.GetField<string>("Status");
                var CurrentLevel = csv.GetField<int?>("CurrentLevel");
                var ScoreCurrent = csv.GetField<long?>("ScoreCurrent");
                var DeviceOs = csv.GetField<string>("DeviceOs");
                var ClientVersion = csv.GetField<string>("ClientVersion");
                sessions.Add(new GameSession
                {
                    Id = Guid.NewGuid(),
                    UserId = users[userIdx].Id,
                    GameId = games[gameIdx].Id,
                    Status = Status,
                    CurrentLevel = CurrentLevel,
                    ScoreCurrent = ScoreCurrent,
                    DeviceOs = DeviceOs,
                    ClientVersion = ClientVersion,
                    StartedAt = DateTime.UtcNow.AddHours(startOff),
                    EndedAt = endedAt,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                break;
            }
            case "Log":
            {
                var sessionIdx = csv.GetField<int>("SessionIndex");
                var snapshotRaw = csv.GetField<string>("StateSnapshot");
                JsonDocument? snapshot = snapshotRaw is { Length: > 0 }
                    ? JsonDocument.Parse(snapshotRaw)
                    : null;
                logs.Add(new SessionLog
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessions[sessionIdx].Id,
                    EventType = csv.GetField<string>("EventType"),
                    SeverityLevel = csv.GetField<string>("SeverityLevel"),
                    IpAddress = csv.GetField<string>("IpAddress"),
                    LatencyMs = csv.GetField<int?>("LatencyMs"),
                    StateSnapshot = snapshot,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                break;
            }
        }
    }
}

await using var transaction = await db.Database.BeginTransactionAsync();
db.Users.AddRange(users);
db.Games.AddRange(games);
await db.SaveChangesAsync();
Console.WriteLine($"Inserted {users.Count} users and {games.Count} games.");

db.GameSessions.AddRange(sessions);
await db.SaveChangesAsync();
Console.WriteLine($"Inserted {sessions.Count} game sessions.");

db.SessionLogs.AddRange(logs);

await db.SaveChangesAsync();
Console.WriteLine($"Inserted {logs.Count} session logs.");
await transaction.CommitAsync();
Console.WriteLine("Done — mock data inserted successfully.");