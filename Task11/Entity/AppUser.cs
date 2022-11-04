namespace Task11.Entity;

public class AppUser
{
    public uint Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Key { get; set; }
    public ERole Role { get; set; }
    public object Email { get; internal set; }
}
