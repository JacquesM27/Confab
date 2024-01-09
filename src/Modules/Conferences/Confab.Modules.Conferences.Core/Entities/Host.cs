namespace Confab.Modules.Conferences.Core.Entities;

public class Host
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<Conference> Conferences { get; set; } = new List<Conference>();
}