using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Task11.Entity;

namespace Task11.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IOptions<AppSettings> _options;

    public UserService(
        ILogger<UserService> logger,
        IOptions<AppSettings> options)
    {
        _logger = logger;
        _options = options;
    }

    public bool InsertUser(string? login, string? password)
    {
        if(string.IsNullOrWhiteSpace(login)) return false;
        if(string.IsNullOrWhiteSpace(password)) return false;
        
        var existingUser = RetrieveUserByLogin(login);
        if(existingUser is not null) return false;

        var entity = new AppUser()
        {
            Login = login,
            Password = password,
            Key = Guid.NewGuid().ToString(),
            Role = ERole.User
        };

        var insertedResult = WriteDataToJson(entity);

        if (insertedResult is false) return false;

        return true;
    }

    public AppUser? RetrieveUserByKey(string? key)
    {
        if (string.IsNullOrWhiteSpace(key)) return null;

        CheckAndCreateDatabase();

        var users = ReadUsersFromJson();
        if (users.Count == 0) return null;

        var userEntity = users.FirstOrDefault(u => u.Key == key);
        if (userEntity is null) return null;

        return userEntity;
    }

    public AppUser? RetrieveUserByLogin(string? login)
    {
        if(string.IsNullOrWhiteSpace(login)) return null;
        
        CheckAndCreateDatabase();
        
        var users = ReadUsersFromJson();
        if(users.Count == 0) return null;

        var userEntity = users.FirstOrDefault(u => u.Login == login);
        if (userEntity is null) return null;

        return userEntity;
    }

    public bool UpdateUser(string? login, string? password, ERole role)
    {
        if (string.IsNullOrWhiteSpace(login)) return false;
        if (string.IsNullOrWhiteSpace(password)) return false;

        var user = RetrieveUserByLogin(login);
        if(user is null) return false;

        user.Login = login;
        user.Password = password;
        user.Role = role;
        
        var updatedResult = WriteDataToJson(user);
        
        if (updatedResult is false) return false;

        return true;
    }

    private bool WriteDataToJson(AppUser user)
    {
        var directory = Directory.GetCurrentDirectory();
        var path = Path.Combine(directory, "Data", _options.Value.JsonDataPath ?? String.Empty);
        CheckAndCreateDatabase();
        var usersList = ReadUsersFromJson();
        usersList.Add(user);
        return true;
    }

    private void CheckAndCreateDatabase()
    {
        var directory = Directory.GetCurrentDirectory();
        var path = Path.Combine(directory, "Data", _options.Value.JsonDataPath ?? String.Empty);
        if (!System.IO.File.Exists(path))
            System.IO.File.Create(path);
    }
    private List<AppUser>? ReadUsersFromJson()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", _options.Value.JsonDataPath ?? String.Empty);

        if(!System.IO.File.Exists(path))
            return new List<AppUser>();

        var json = System.IO.File.ReadAllText(path);

        try
        {
            List<AppUser>? users = JsonConvert.DeserializeObject<List<AppUser>>(json);
            return users;
        }
        catch (Exception exception)
        {
            _logger.LogInformation($"Cannot deserialize json {exception.InnerException}");
            return new List<AppUser>();
        }
    }
}
