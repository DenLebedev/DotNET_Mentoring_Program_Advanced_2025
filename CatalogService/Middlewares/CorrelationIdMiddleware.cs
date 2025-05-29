using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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

        var activity = new Activity("Incoming Request");
        activity.SetIdFormat(ActivityIdFormat.W3C);
        activity.SetParentId(Activity.Current?.Id);
        activity.Start();
        activity.AddTag("CorrelationId", correlationId);

        context.Items[CorrelationHeader] = correlationId;

        using (activity)
        {
            await _next(context);
        }
    }
}
