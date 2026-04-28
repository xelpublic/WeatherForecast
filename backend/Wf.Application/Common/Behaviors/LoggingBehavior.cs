using MediatR;
using Serilog;
using Wf.Application.Interfaces;

namespace Wf.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest
    : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        Log.Information("Запрос Wf: {Name} {@Request}",
            requestName, request);

        var response = await next();

        return response;
    }
}