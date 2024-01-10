using System.ComponentModel.DataAnnotations;

namespace Confab.Modules.Conferences.Core.DTO;

internal class ConferenceDto
{
    public Guid Id { get; set; }    
    public Guid HostId { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
    public string? HostName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Location { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;
    public int? ParticipantsLimit { get; set; }
    
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}
