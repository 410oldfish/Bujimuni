using System;
using System.Diagnostics;
using Lighten;
using Debug = UnityEngine.Debug;

public class TimeMonitor : IDisposable
{
    private string m_title;
    private Stopwatch m_stopwatch;

    public TimeMonitor()
    {
        this.m_stopwatch = new Stopwatch();
    }
    
    private void StartInternal(string title)
    {
        this.m_title = title;
        this.m_stopwatch.Restart();
    }
    
    public void Dispose()
    {
        this.m_stopwatch.Stop();
        Debug.Log($"<color=#ffff00>{this.m_title} time:{this.m_stopwatch.Elapsed.TotalSeconds}s</color>");
        ObjectPool.Recycle(this);
    }
    
    public static TimeMonitor Start(string title)
    {
        var timeMonitor = ObjectPool.Fetch<TimeMonitor>();
        timeMonitor.StartInternal(title);
        return timeMonitor;
    }
}
