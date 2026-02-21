using Lab2.Models;
namespace Lab2.Repositories;
public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();
    public Task<IEnumerable<User>> GetAllAsync()
        => Task.FromResult<IEnumerable<User>>(_users.ToList());
    public Task<User?> GetByIdAsync(Guid id)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    public Task<User?> GetByEmailAsync(string email)
        => Task.FromResult(_users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
    public Task<User?> GetByUsernameAsync(string username)
        => Task.FromResult(_users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));
    public Task<User> CreateAsync(User user)
    {
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        _users.Add(user);
        return Task.FromResult(user);
    }
    public Task<User?> UpdateAsync(Guid id, User updated)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user is null) return Task.FromResult<User?>(null);
        user.Username = updated.Username;
        user.Email = updated.Email;
        if (!string.IsNullOrWhiteSpace(updated.PasswordHash))
            user.PasswordHash = updated.PasswordHash;
        return Task.FromResult<User?>(user);
    }
    public Task<bool> DeleteAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user is null) return Task.FromResult(false);
        _users.Remove(user);
        return Task.FromResult(true);
    }
    public Task<bool> ExistsByEmailAsync(string email)
        => Task.FromResult(_users.Any(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
    public Task<bool> ExistsByUsernameAsync(string username)
        => Task.FromResult(_users.Any(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));
}