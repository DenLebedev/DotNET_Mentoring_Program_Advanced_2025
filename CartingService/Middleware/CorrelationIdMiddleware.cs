using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Serilog.Context;

namespace CartingService.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers.TryGetValue(CorrelationHeader, out var headerVal)
                ? headerVal.ToString()
                : Guid.NewGuid().ToString();

            context.Response.Headers[CorrelationHeader] = correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}

