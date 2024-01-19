using Confab.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Http;

namespace Confab.Shared.Infrastructure.Contexts;

internal class ContextFactory(IHttpContextAccessor httpContextAccessor) : IContextFactory
{
    public IContext Create()
    {
        var httpContext = httpContextAccessor.HttpContext;

        return httpContext is null ? Context.Empty : new Context(httpContext);
    }
}