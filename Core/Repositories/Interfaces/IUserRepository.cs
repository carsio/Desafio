using Core.Models;

namespace Core.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);
    Task<User> Create(User user);
    Task<User> Update(User user);
}