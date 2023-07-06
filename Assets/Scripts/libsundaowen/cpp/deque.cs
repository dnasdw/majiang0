using System.Collections.Generic;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public class dequep<T> : LinkedList<T>, IContainer<T>, IQueueContainer<T>, IStackContainer<T>
        {
            public T front()
            {
                return base.First.Value;
            }

            public T back()
            {
                return base.Last.Value;
            }

            public bool empty()
            {
                return base.Count == 0;
            }

            public int size()
            {
                return base.Count;
            }

            public void push_back(T a_Value)
            {
                base.AddLast(a_Value);
            }

            public void pop_back()
            {
                base.RemoveLast();
            }

            public void push_front(T a_Value)
            {
                base.AddFirst(a_Value);
            }

            public void pop_front()
            {
                base.RemoveFirst();
            }
        }
    }
}
