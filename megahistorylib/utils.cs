
using System;

namespace megahistorylib
{
	/// <summary>
	/// 
	/// </summary>
	public class Timer
	{
		private long _start =0;
		private long _delta =0;
		private long _total =0;
		
		/// <summary>
		/// 
		/// </summary>
		public long DeltaT { get { return _delta; } }
		/// <summary>
		/// 
		/// </summary>
		public long TotalT { get { return _total; } set { _total = value; } }
		/// <summary>
		/// 
		/// </summary>
		public TimeSpan Delta { get { return new TimeSpan(_delta); } }
		/// <summary>
		/// 
		/// </summary>
		public TimeSpan Total { get { return new TimeSpan(_total); } }
		
		/// <summary>
		/// 
		/// </summary>
		public Timer() { }
		
		/// <summary>
		/// 
		/// </summary>
		public void start() { _start = DateTime.Now.Ticks; }
		/// <summary>
		/// 
		/// </summary>
		public void stop()
		{
			long now = DateTime.Now.Ticks;
			_delta = now -_start;
			_total += _delta;
		}
	}
}
