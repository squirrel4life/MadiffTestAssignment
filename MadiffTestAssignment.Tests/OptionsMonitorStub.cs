using Microsoft.Extensions.Options;

namespace MadiffTestAssignment.Tests;

public class OptionsMonitorStub<T>(IOptions<T> options) : IOptionsMonitor<T> where T : class
{
    private readonly T _value = options.Value;

    public T CurrentValue => _value;

    public T Get(string name) => _value;

    public IDisposable OnChange(Action<T, string> listener) => new DummyDisposable();

    private class DummyDisposable : IDisposable
    {
        public void Dispose() { }
    }
}