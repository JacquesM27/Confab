using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Exceptions;
using Shouldly;

namespace Confab.Modules.Attendances.Tests.Unit.Entities;

public class Participant_Attend_Tests
{
    private void Act(Attendance attendance) => _participant.Attend(attendance);

    [Fact]
    public void given_the_same_agendaItem_attend_should_fail()
    {
        // Arrange
        var attendance1 = new Attendance(Guid.NewGuid(), _agendaItemId, Guid.NewGuid(), _participant.Id, GetDate(9), GetDate(10));
        var attendance2 = new Attendance(Guid.NewGuid(), _agendaItemId, Guid.NewGuid(), _participant.Id, GetDate(9), GetDate(10));
        _participant.Attend(attendance1);

        // Act
        var exception = Record.Exception(() => Act(attendance2));

        // Asset
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<AlreadyParticipatingInEventException>();
    }
    
    [Theory]
    [MemberData(nameof(GetCollidingDates))]
    public void given_attendance_with_colliding_time_attend_should_fail(DateTime from, DateTime to)
    {
        // Arrange
        var attendance1 = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from, to);
        var attendance2 = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), GetDate(9,30), GetDate(11));
        _participant.Attend(attendance1);

        // Act
        var exception = Record.Exception(() => Act(attendance2));

        // Asset
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<AlreadyParticipatingSameTimeException>();
    }
    
    [Theory]
    [MemberData(nameof(GetAvailableDates))]
    public void given_no_colliding_attendance_attend_should_succeed(DateTime from, DateTime to)
    {
        // Arrange
        var attendance1 = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from, to);
        var attendance2 = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, GetDate(9, 30), GetDate(11));
        _participant.Attend(attendance1);

        // Act
        Act(attendance2);

        // Asset
        _participant.Attendances.ShouldNotBeNull();
        _participant.Attendances.ShouldContain(attendance2);
    }

    private readonly Participant _participant;

    public Participant_Attend_Tests()
    {
        _participant = new Participant(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    }
    
    private Guid _agendaItemId = Guid.NewGuid();

    private static DateTime GetDate(int hour, int minute = 0, int second = 0)
        => new (2024, 2, 6, hour, minute, second);
    
    
    public static IEnumerable<object[]> GetCollidingDates()
    {
        yield return new object[] {GetDate(9), GetDate(10)};
        yield return new object[] {GetDate(9), GetDate(11, 30)};
        yield return new object[] {GetDate(10), GetDate(10, 30)};
        yield return new object[] {GetDate(10), GetDate(12)};
    }

    public static IEnumerable<object[]> GetAvailableDates()
    {
        yield return new object[] {GetDate(8), GetDate(9)};
        yield return new object[] {GetDate(8), GetDate(9, 30)};
        yield return new object[] {GetDate(11), GetDate(12)};
        yield return new object[] {GetDate(12), GetDate(13)};
    }
}