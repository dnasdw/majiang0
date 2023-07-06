using System.Collections.Generic;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public interface IContainer<T> : IEnumerable<T>
        {
            public bool empty();

            public int size();
        }
    }
}
