using Confab.Shared.Abstractions.Exceptions;
using Humanizer;
using System.Collections.Concurrent;

namespace Confab.Shared.Infrastructure.Exceptions
{
    internal class ExceptionsToResponseMapper : IExceptionsToResponseMapper
    {
        private static readonly ConcurrentDictionary<Type, string> Codes = new();

        public ExceptionResponse? Map(Exception exception)
            => exception switch
            {
                ConfabException ex => new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), System.Net.HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(new ErrorsResponse(new Error("error", "There was an error.")) , System.Net.HttpStatusCode.InternalServerError)
                // TODO: for system exceptions add some ticket id.
            };

        private static string GetErrorCode(object exception)
        {
            var type = exception.GetType();
            return Codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
        }

        private record Error(string Code, string Message);
        private record ErrorsResponse(params Error[] Errors);
    }
}
