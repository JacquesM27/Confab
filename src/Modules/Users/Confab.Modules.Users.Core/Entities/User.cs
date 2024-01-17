namespace Confab.Modules.Users.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Dictionary<string, IEnumerable<string>> Claims { get; set; }
        = new Dictionary<string, IEnumerable<string>>();
}