using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;
using Confab.Modules.Attendances.Application.Events.External;
using Confab.Modules.Attendances.Application.Events.External.Handlers;
using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Exceptions;
using Confab.Modules.Attendances.Domain.Policies;
using Confab.Modules.Attendances.Domain.Repositories;
using Confab.Shared.Abstractions.Events;
using NSubstitute;
using Shouldly;

namespace Confab.Modules.Attendances.Tests.Unit.Events.Handlers;

public class AgendaItemAssignedToAgendaSlotHandlerTests
{
    private Task Act(AgendaItemAssignedToAgendaSlot @event) => _handler.HandleAsync(@event);

    [Fact]
    public async Task given_already_existing_attendable_event_new_one_should_not_be_created()
    {
        // Arrange
        var attendableEvent = GetAttendableEvent();
        var @event = new AgendaItemAssignedToAgendaSlot(Guid.NewGuid(), attendableEvent.Id);
        _attendableEventRepository.GetAsync(@event.AgendaItemId).Returns(attendableEvent);

        // Act
        await Act(@event);

        // Assert
        await _attendableEventRepository.Received(1).GetAsync(@event.AgendaItemId);
        await _agendasApiClient.DidNotReceiveWithAnyArgs().GetRegularAgendaSlotAsync(default);
        await _attendableEventRepository.DidNotReceiveWithAnyArgs().AddAsync(default);
    }

    [Fact]
    public async Task given_missing_regular_agenda_slot_handler_should_fail()
    {
        // Arrange
        var @event = new AgendaItemAssignedToAgendaSlot(Guid.NewGuid(), Guid.NewGuid());
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(@event));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<AttendableEventNotFoundException>();
        await _attendableEventRepository.Received(1).GetAsync(@event.AgendaItemId);
        await _agendasApiClient.Received(1).GetRegularAgendaSlotAsync(@event.AgendaItemId);
        await _attendableEventRepository.DidNotReceiveWithAnyArgs().AddAsync(default);
    }

    [Fact]
    public async Task given_no_participants_limit_attendable_event_should_not_be_added()
    {
        // Arrange
        var @event = new AgendaItemAssignedToAgendaSlot(Guid.NewGuid(), Guid.NewGuid());
        var agendaSlotDto = GetRegularAgendaSlotDto();
        _agendasApiClient.GetRegularAgendaSlotAsync(@event.AgendaItemId).Returns(agendaSlotDto);
        
        // Act
        await Act(@event);

        // Assert
        await _attendableEventRepository.Received(1).GetAsync(@event.AgendaItemId);
        await _agendasApiClient.Received(1).GetRegularAgendaSlotAsync(@event.AgendaItemId);
        await _attendableEventRepository.DidNotReceiveWithAnyArgs().AddAsync(default);
    }

    [Fact]
    public async Task given_participants_limit_attendable_event_should_be_added()
    {
        // Arrange
        const int participantsLimit = 100;
        var slots = Enumerable.Range(0, 100).Select(x => new Slot(Guid.NewGuid()));
        var @event = new AgendaItemAssignedToAgendaSlot(Guid.NewGuid(), Guid.NewGuid());
        var agendaSlotDto = GetRegularAgendaSlotDto(participantsLimit);
        var tags = agendaSlotDto.AgendaItem.Tags.ToArray();
        _agendasApiClient.GetRegularAgendaSlotAsync(@event.AgendaItemId).Returns(agendaSlotDto);
        _slotPolicyFactory.Get(tags).Returns(_slotPolicy);
        _slotPolicy.Generate(participantsLimit).Returns(slots);

        // Act
        await Act(@event);

        // Assert
        await _attendableEventRepository.Received(1).GetAsync(@event.AgendaItemId);
        await _agendasApiClient.Received(1).GetRegularAgendaSlotAsync(@event.AgendaItemId);
        _slotPolicyFactory.Received(1).Get(tags);
        _slotPolicy.Received(1).Generate(participantsLimit);
        await _attendableEventRepository.Received(1)
            .AddAsync(Arg.Is<AttendableEvent>(x => x.Id == @event.AgendaItemId
                                                   && x.ConferenceId == agendaSlotDto.AgendaItem.ConferenceId
                                                   && x.From == agendaSlotDto.From
                                                   && x.To == agendaSlotDto.To));
    }
    
    private readonly IAttendableEventRepository _attendableEventRepository;
    private readonly ISlotPolicyFactory _slotPolicyFactory;
    private readonly IAgendasApiClient _agendasApiClient;
    private readonly ISlotPolicy _slotPolicy;
    
    private readonly IEventHandler<AgendaItemAssignedToAgendaSlot> _handler;

    public AgendaItemAssignedToAgendaSlotHandlerTests()
    {
        _attendableEventRepository = Substitute.For<IAttendableEventRepository>();
        _slotPolicyFactory = Substitute.For<ISlotPolicyFactory>();
        _agendasApiClient = Substitute.For<IAgendasApiClient>();
        _slotPolicy = Substitute.For<ISlotPolicy>();
        
        _handler = new AgendaItemAssignedToAgendaSlotHandler(_attendableEventRepository, _slotPolicyFactory, 
            _agendasApiClient);
    }

    private static AttendableEvent GetAttendableEvent()
        => new(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

    private static RegularAgendaSlotDto GetRegularAgendaSlotDto(int? participantsLimit = null)
        => new()
        {
            Id = Guid.NewGuid(),
            ParticipantsLimit = participantsLimit,
            From = DateTime.UtcNow,
            To = DateTime.UtcNow.AddDays(1),
            AgendaItem = new AgendaItemDto()
            {
                ConferenceId = Guid.NewGuid(),
                Tags = ["tag1", "tag2"]
            }
        };
}