using System.Diagnostics;

namespace Starter.Application.Interfaces;
public interface ITracingService
{
    Activity? StartActivity(string name);
    Activity? StartActivity(string name, ActivityKind kind);
    void EnrichCurrentActivity(string key, object? value);
    void SetError(Exception exception);
}
