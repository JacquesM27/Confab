namespace Confab.Shared.Abstractions.Exceptions
{
    public interface IExceptionsToResponseMapper
    {
        ExceptionResponse? Map(Exception exception);
    }
}
