using System.Collections;
using System.Collections.Generic;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public class stackp<T> : stackp<T, dequep<T>>
        {
            public stackp()
                : base()
            {
            }

            public stackp(IEnumerable<T> a_Cont)
                : base(a_Cont)
            {
            }

            public stackp(dequep<T> a_Cont)
                : base(a_Cont)
            {
            }
        }

        public class stackp<T, Container> : IContainer<T> where Container : class, IStackContainer<T>, new()
        {
            private Container m_Container = null;

            public stackp()
            {
                m_Container = new Container();
            }

            public stackp(IEnumerable<T> a_Cont)
                : this()
            {
                for (IEnumerator<T> it = a_Cont.GetEnumerator(); it.MoveNext(); /**/)
                {
                    T current = it.Current;
                    push(current);
                }
            }

            public stackp(Container a_Cont)
                : this(reinterpret_cast<IEnumerable<T>>(a_Cont))
            {
            }

            public T top()
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
                m_Container.pop_back();
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
