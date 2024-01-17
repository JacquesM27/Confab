using System.ComponentModel.DataAnnotations;

namespace Confab.Modules.Users.Core.DTO;

public class SignUpDto
{
    public Guid Id { get; set; }
    
    [EmailAddress]
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public Dictionary<string, IEnumerable<string>> Claims { get; set; }
        = new Dictionary<string, IEnumerable<string>>();
}