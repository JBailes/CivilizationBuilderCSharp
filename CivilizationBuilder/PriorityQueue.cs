using System;
using System.Collections.Generic;
using System.Linq;

namespace CivilizationBuilder
{
	class PriorityQueue<T>
	{
		List<Tuple<T, int>> queue = new List<Tuple<T,int>>();

		public T Dequeue()
		{
			var result = queue[0];
			queue.RemoveAt(0);

			return result.Item1;
		}

		public void Enqueue(T item, int priority)
		{
			for (int i = 0; i < queue.Count; i++)
			{
				if (queue[i].Item2 >= priority)
				{
					queue.Insert(i, new Tuple<T, int>(item, priority));
					return;
				}
			}
			
			// Cases of last in queue, or empty queue
			queue.Insert(queue.Count, new Tuple<T, int>(item, priority));
		}

		public bool HasNext()
		{
			return queue.Count > 0;
		}
	}
}
