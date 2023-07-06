using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public static pairp<T1, T2> make_pair<T1, T2>(T1 a_T, T2 a_U)
        {
            return new pairp<T1, T2>(a_T, a_U);
        }

        public class pairp<T1, T2>
        {
            public T1 first;
            public T2 second;

            public pairp()
            {
                first = CDefault.Instance.GetDefaultValue<T1>();
                second = CDefault.Instance.GetDefaultValue<T2>();
            }

            public pairp(T1 a_X, T2 a_Y)
            {
                first = a_X;
                second = a_Y;
            }
        }
    }
}
