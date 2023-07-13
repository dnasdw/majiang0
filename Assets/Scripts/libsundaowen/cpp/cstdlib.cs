using System;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        private static Random s_Random = new Random(1);

        public static int rand()
        {
            return s_Random.Next();
        }

        public static void srand(uint a_uSeed)
        {
            s_Random = new Random(static_cast_slow<int>(a_uSeed));
        }

        public static int abs(int a_nN)
        {
            return Math.Abs(a_nN);
        }

        public static long abs(long a_nN)
        {
            return Math.Abs(a_nN);
        }

        public static long labs(long a_nN)
        {
            return Math.Abs(a_nN);
        }

        public static long llabs(long a_nN)
        {
            return Math.Abs(a_nN);
        }
    }
}
