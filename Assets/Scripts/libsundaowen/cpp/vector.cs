using System.Collections.Generic;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public class vectorp<T> : List<T>, IContainer<T>, IStackContainer<T>
        {
            public vectorp()
            {
            }

            public vectorp(int a_nCount)
                : base(a_nCount)
            {
                CDefault defaultInstance = CDefault.Instance;
                for (int i = 0; i < a_nCount; i++)
                {
                    // do not cache
                    push_back(defaultInstance.GetDefaultValue<T>());
                }
            }

            public vectorp(int a_nCount, T a_Value)
                : base(a_nCount)
            {
                for (int i = 0; i < a_nCount; i++)
                {
                    push_back(a_Value);
                }
            }

            public vectorp(vectorp<T> other)
                : base(other)
            {
            }

            public T front()
            {
                return base[0];
            }

            public T back()
            {
                return base[size() - 1];
            }

            public bool empty()
            {
                return base.Count == 0;
            }

            public int size()
            {
                return base.Count;
            }

            public void reserve(int a_nNewCap)
            {
                base.Capacity = a_nNewCap;
            }

            public int capacity()
            {
                return base.Capacity;
            }

            public void clear()
            {
                base.Clear();
            }

            public void push_back(T a_Value)
            {
                base.Add(a_Value);
            }

            public void pop_back()
            {
                base.RemoveAt(base.Count - 1);
            }
        }
    }
}
