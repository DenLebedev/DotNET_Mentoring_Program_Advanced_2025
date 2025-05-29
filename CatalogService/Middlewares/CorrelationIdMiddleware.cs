using System.Diagnostics;
using Serilog.Context;

public class CorrelationIdMiddleware
{
    private const string CorrelationHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.ContainsKey(CorrelationHeader)
            ? context.Request.Headers[CorrelationHeader].ToString()
            : Guid.NewGuid().ToString();

        context.Response.Headers[CorrelationHeader] = correlationId;
        context.Items[CorrelationHeader] = correlationId;

        // Add to Activity and Baggage for trace propagation
        var activity = new Activity("Incoming Request");
        activity.SetIdFormat(ActivityIdFormat.W3C);
        activity.SetParentId(Activity.Current?.Id);
        activity.Start();
        activity.AddTag("CorrelationId", correlationId);
        activity.AddBaggage("CorrelationId", correlationId);

        using (activity)
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
