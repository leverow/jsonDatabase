using Task11.Entity;

namespace Task11.Services;

public interface IUserService
{
    bool InsertUser(string? login, string? password);
    bool UpdateUser(string? login, string? password, ERole role);
    AppUser? RetrieveUserByLogin(string? login);
    AppUser? RetrieveUserByKey(string? key);
}
