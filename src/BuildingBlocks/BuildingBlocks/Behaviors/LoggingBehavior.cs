﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}", typeof(TRequest).Name, typeof(TResponse).Name, request);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            TResponse response = await next();

            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3)
                logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.", typeof(TRequest).Name, timeTaken.Seconds);

            logger.LogInformation("[END] Handled {Request} with {Response}.", typeof(TRequest).Name, typeof(TResponse).Name);

            return response;
        }
    }
}
