using MaximTechnologyTasks.Configs;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

namespace MaximTechnologyTasks.Middlewares
{
    public class MaxConcurrentRequestsMiddleware
    {
        private int _concurrentRequestsCount;

        private readonly RequestDelegate _next;
        private readonly ConcurrentReqestsSetting _options;

        public MaxConcurrentRequestsMiddleware(RequestDelegate next, ConcurrentReqestsSetting options)
        {
            _concurrentRequestsCount = 0;
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (CheckLimitExceeded())
            {
                IHttpResponseFeature responseFeature = context.Features.Get<IHttpResponseFeature>();

                responseFeature.StatusCode = StatusCodes.Status503ServiceUnavailable;
                responseFeature.ReasonPhrase = "Concurrent request limit exceeded.";
            }
            else
            {
                await _next(context);

                Interlocked.Decrement(ref _concurrentRequestsCount);
            }
        }

        private bool CheckLimitExceeded()
        {
            bool limitExceeded;
            int initialConcurrentRequestsCount, incrementedConcurrentRequestsCount;
            do
            {
                limitExceeded = true;

                initialConcurrentRequestsCount = _concurrentRequestsCount;
                if (initialConcurrentRequestsCount >= _options.ConcurrentRequestsLimit)
                {
                    break;
                }

                limitExceeded = false;
                incrementedConcurrentRequestsCount = initialConcurrentRequestsCount + 1;
            }
            while (initialConcurrentRequestsCount != Interlocked.CompareExchange(
            ref _concurrentRequestsCount, incrementedConcurrentRequestsCount, initialConcurrentRequestsCount));

            return limitExceeded;
        }
    }
}
