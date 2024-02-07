using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;
using Confab.Modules.Attendances.Application.DTO;
using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Infrastructure.EF;
using Confab.Modules.Attendances.Test.Integration.Common;
using Confab.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;

namespace Confab.Modules.Attendances.Test.Integration.Controllers;

[Collection("integration")]
public class AttendancesControllerTests : 
    IClassFixture<TestApplicationFactory>, 
    IClassFixture<TestAttendancesDbContext>
{
    [Fact]
    public async Task get_browse_attendances_without_being_authorized_should_return_unauthorized_http_status_code()
    {
        // Arrange
        var conferenceId = Guid.NewGuid();
        
        // Act
        var response = await _client.GetAsync($"{Path}/{conferenceId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task get_browse_attendances_given_invalid_participant_should_return_not_found()
    {
        // Arrange
        var conferenceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        Authenticate(userId);

        // Act
        var response = await _client.GetAsync($"{Path}/{conferenceId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task get_browse_attendances_given_valid_conference_and_participant_should_return_all_attendances()
    {
        // Arrange
        var from = DateTime.UtcNow;
        var to = from.AddDays(1);
        var conferenceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var participant = new Participant(Guid.NewGuid(), conferenceId, userId);
        var slot = new Slot(Guid.NewGuid(), participant.Id);
        var attendableEvent = new AttendableEvent(Guid.NewGuid(), conferenceId, from, to, [slot]);
        var attendance = new Attendance(Guid.NewGuid(), attendableEvent.Id, slot.Id, participant.Id, from, to);

        await _dbContext.AttendableEvents.AddAsync(attendableEvent);
        await _dbContext.Attendances.AddAsync(attendance);
        await _dbContext.Participants.AddAsync(participant);
        await _dbContext.Slots.AddAsync(slot);
        await _dbContext.SaveChangesAsync();

        _agendasApiClient.GetAgendaAsync(conferenceId).Returns([new AgendaTrackDto
            {
                Id = Guid.NewGuid(),
                Name = "Track 1",
                ConferenceId = conferenceId,
                Slots = [new RegularAgendaSlotDto()
                {
                    Id = Guid.NewGuid(),
                    From = from,
                    To = to,
                    AgendaItem = new AgendaItemDto()
                    {
                        Id = attendableEvent.Id,
                        From = from,
                        To = to,
                        Title = "test",
                        Description = "test",
                        Level = 1
                    }
                }]
            }
        ]);
        
        Authenticate(userId);

        // Act
        var response = await _client.GetAsync($"{Path}/{conferenceId}");
        
        // Assert
        response.IsSuccessStatusCode.ShouldBeTrue();

        var attendances = await response.Content.ReadFromJsonAsync<AttendanceDto[]>();
        attendances.ShouldNotBeEmpty();
        attendances.Length.ShouldBe(1);
    }

    [Fact]
    public async Task post_attend_should_succeed_free_slots_and_valid_participant()
    {
        // Arrange
        var from = DateTime.UtcNow;
        var to = from.AddDays(1);
        var conferenceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var participant = new Participant(Guid.NewGuid(), conferenceId, userId);
        var slot = new Slot(Guid.NewGuid());
        var attendableEvent = new AttendableEvent(Guid.NewGuid(), conferenceId, from, to, [slot]);

        await _dbContext.AttendableEvents.AddAsync(attendableEvent);
        await _dbContext.Participants.AddAsync(participant);
        await _dbContext.Slots.AddAsync(slot);
        await _dbContext.SaveChangesAsync();
        
        Authenticate(userId);
        
        // Act
        var response = await _client.PostAsJsonAsync($"{Path}/events/{attendableEvent.Id}/attend", new {});

        // Assert
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
        
    private void Authenticate(Guid userId)
    {
        var jwt = AuthHelper.CreateJwt(userId.ToString());
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }
    
    private const string Path = "attendances-module/attendances";
    private HttpClient _client;
    private AttendancesDbContext _dbContext;

    private readonly IAgendasApiClient _agendasApiClient;
    
    public AttendancesControllerTests(TestApplicationFactory factory, TestAttendancesDbContext dbContext)
    {
        _agendasApiClient = Substitute.For<IAgendasApiClient>();
        _client = factory
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                services.AddSingleton(_agendasApiClient);
            }))
            .CreateClient();
        _dbContext = dbContext.DbContext;
    }
}