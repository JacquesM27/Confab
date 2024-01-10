namespace Confab.Modules.Conferences.Core.Entities;

public class Conference
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public Host? Host { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public int? ParticipantsLimit { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}