using System.Collections;
using System.Collections.Generic;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public class queuep<T> : queuep<T, dequep<T>>
        {
            public queuep()
                : base()
            {
            }

            public queuep(IEnumerable<T> a_Cont)
                : base(a_Cont)
            {
            }

            public queuep(dequep<T> a_Cont)
                : base(a_Cont)
            {
            }
        }

        public class queuep<T, Container> : IContainer<T> where Container : class, IQueueContainer<T>, new()
        {
            private Container m_Container = null;

            public queuep()
            {
                m_Container = new Container();
            }

            public queuep(IEnumerable<T> a_Cont)
                : this()
            {
                for (IEnumerator<T> it = a_Cont.GetEnumerator(); it.MoveNext(); /**/)
                {
                    T current = it.Current;
                    push(current);
                }
            }

            public queuep(Container a_Cont)
                : this(reinterpret_cast<IEnumerable<T>>(a_Cont))
            {
            }

            public T front()
            {
                return m_Container.front();
            }

            public T back()
            {
                return m_Container.back();
            }

            public bool empty()
            {
                return m_Container.empty();
            }

            public int size()
            {
                return m_Container.size();
            }

            public void push(T a_Value)
            {
                m_Container.push_back(a_Value);
            }

            public void pop()
            {
                m_Container.pop_front();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return m_Container.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return m_Container.GetEnumerator();
            }
        }
    }
}
