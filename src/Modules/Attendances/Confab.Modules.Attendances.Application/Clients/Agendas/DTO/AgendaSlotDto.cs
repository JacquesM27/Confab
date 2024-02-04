namespace Confab.Modules.Attendances.Application.Clients.Agendas.DTO;

public class AgendaSlotDto
{
    public Guid Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Type { get; set; }
}