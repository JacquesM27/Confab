namespace Confab.Modules.Users.Core.DTO;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public Dictionary<string, IEnumerable<string>> Claims { get; set; }
        = new Dictionary<string, IEnumerable<string>>();

    public DateTime CreatedAt { get; set; }
}