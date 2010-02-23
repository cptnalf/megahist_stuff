
using System.Collections.Generic;
using System.Threading;

namespace StarTree.Plugin
{
	/// <summary>
	/// async queue with a contained semaphore, signaling how many available items there are
	/// in the queue.
	/// all functions/properties of this object are synchronized.
	/// </summary>
	/// <typeparam name="T">any type will suffice</typeparam>
    public class AsyncQueue<T>
    {
			private Queue<T> _q;
			private Semaphore _itemsWaiting;

			public int Count
			{
				get
					{
						int res = 0;
						lock (((System.Collections.ICollection)_q).SyncRoot)
							{ res = _q.Count; }
						
						return res;
					}
			}
			
			public Semaphore ItemsWaiting
			{ get { return _itemsWaiting; } }
      
			public AsyncQueue() : this(System.Int32.MaxValue) { }
      
			public AsyncQueue(int maxWaiting)
      {
				_itemsWaiting = new Semaphore(0, maxWaiting); /* look at this at a later date. */
        
				_q = new Queue<T>();
			}
			
			/// <summary>
			/// automagically deals with the semaphore.
			/// </summary>
			/// <param name="item"></param>
			public void push(T item)
			{
				lock (((System.Collections.ICollection)_q).SyncRoot)
					{
						_q.Enqueue(item);
					}
				_itemsWaiting.Release();
			}
			
			/// <summary>
			/// does not do anything with the semaphore.
			/// </summary>
			/// <returns></returns>
			public T pop()
			{
				T item = default(T);
				
				lock (((System.Collections.ICollection)_q).SyncRoot)
					{
						if(_q.Count > 0)
							{ item = _q.Dequeue(); }
					}
				return item;
			}
    }
}

