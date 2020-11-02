namespace EMS.Integration {
    using System;
    using System.Diagnostics;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
            var requestName = request.GetType().FullName;
            var requestGuid = Guid.NewGuid().ToString();

            var requestNameWithGuid = $"[{requestName} [{requestGuid}]]";

            logger.LogInformation($"{requestNameWithGuid}");
            TResponse response;

            var stopwatch = Stopwatch.StartNew();
            try {
                try {
                    logger.LogInformation($"{requestNameWithGuid} {JsonSerializer.Serialize(request)}");
                }
                catch (NotSupportedException) {
                    logger.LogInformation($"{requestNameWithGuid} Could not serialize the request.");
                }

                response = await next();
            }
            finally {
                stopwatch.Stop();
                logger.LogInformation($"{requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
            }

            return response;
        }
    }
}