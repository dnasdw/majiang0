using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public interface IStackContainer<T> : IContainer<T>
        {
            public T back();

            public void push_back(T a_Value);

            public void pop_back();
        }
    }
}
