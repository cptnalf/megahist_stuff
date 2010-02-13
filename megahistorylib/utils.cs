
using System;

public class Timer
{
	private long _start =0;
	private long _delta =0;
	private long _total =0;
	
	public long DeltaT { get { return _delta; } }
	public long TotalT { get { return _total; } set { _total = value; } }
	public TimeSpan Delta { get { return new TimeSpan(_delta); } }
	public TimeSpan Total { get { return new TimeSpan(_total); } }
	
	public Timer() { }
	
	public void start() { _start = DateTime.Now.Ticks; }
	public void stop()
	{
		long now = DateTime.Now.Ticks;
		_delta = now -_start;
		_total += _delta;
	}
}
