using System.Collections.Generic;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public class setp<T> : SortedSet<T>, IContainer<T>
        {
            public bool empty()
            {
                return base.Count == 0;
            }

            public int size()
            {
                return base.Count;
            }

            public pairp<object, bool> insert(T a_Value)
            {
                pairp<object, bool> pResult = new pairp<object, bool>(null, base.Add(a_Value));
                return pResult;
            }
        }
    }
}
