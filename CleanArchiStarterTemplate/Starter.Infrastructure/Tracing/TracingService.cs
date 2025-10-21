using Starter.Application.Interfaces;
using System.Diagnostics;

namespace Starter.Infrastructure.Tracing;

public class TracingService : ITracingService
{
    private readonly ActivitySource _source;

    public TracingService()
    {
        _source = new ActivitySource("StarterApp", "1.0.0");
    }

    public Activity? StartActivity(string name)
    {
        return _source.StartActivity(name);
    }

    public Activity? StartActivity(string name, ActivityKind kind = ActivityKind.Internal)
    {
        return _source.StartActivity(name, kind);
    }

    public void EnrichCurrentActivity(string key, object? value)
    {
        Activity.Current?.SetTag(key, value);
    }

    public void SetError(Exception exception)
    {
        var activity = Activity.Current;
        if(activity != null)
        {
            activity.SetStatus(ActivityStatusCode.Error, exception.Message);
            activity.SetTag("error.type", exception.GetType().Name);
            activity.SetTag("error.message", exception.Message);
            activity.SetTag("error.stack_trace", exception.StackTrace);
        }
    }
}

