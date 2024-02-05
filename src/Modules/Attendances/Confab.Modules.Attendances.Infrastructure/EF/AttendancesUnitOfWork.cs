using Confab.Shared.Infrastructure.Postgres;

namespace Confab.Modules.Attendances.Infrastructure.EF;

internal class AttendancesUnitOfWork(AttendancesDbContext dbContext) 
    : PostgresUnitOfWork<AttendancesDbContext>(dbContext), IAttendancesUnitOfWork
{
}