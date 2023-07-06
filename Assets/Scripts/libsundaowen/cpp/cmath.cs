using System;

using static sdw.cpp;

using static_cast_int = System.Int32;
using static_cast_long = System.Int64;

namespace sdw
{
    public static partial class std
    {
        public static float abs(float a_fArg)
        {
            return MathF.Abs(a_fArg);
        }

        public static double abs(double a_fArg)
        {
            return Math.Abs(a_fArg);
        }

        public static decimal abs(decimal a_fArg)
        {
            return Math.Abs(a_fArg);
        }

        public static float fabs(float a_fArg)
        {
            return MathF.Abs(a_fArg);
        }

        public static float fabsf(float a_fArg)
        {
            return MathF.Abs(a_fArg);
        }

        public static double fabs(double a_fArg)
        {
            return Math.Abs(a_fArg);
        }

        public static decimal fabs(decimal a_fArg)
        {
            return Math.Abs(a_fArg);
        }

        public static decimal fabsl(decimal a_fArg)
        {
            return Math.Abs(a_fArg);
        }

        public static float lerp(float a_fA, float a_fB, float a_fT)
        {
            const float fOne = 1;
            return a_fA * (fOne - a_fT) + a_fB * a_fT;
        }

        public static double lerp(double a_fA, double a_fB, double a_fT)
        {
            const double fOne = 1;
            return a_fA * (fOne - a_fT) + a_fB * a_fT;
        }

        public static decimal lerp(decimal a_fA, decimal a_fB, decimal a_fT)
        {
            const decimal fOne = 1;
            return a_fA * (fOne - a_fT) + a_fB * a_fT;
        }

        public static float pow(float a_fBase, float a_fExp)
        {
            return MathF.Pow(a_fBase, a_fExp);
        }

        public static double pow(double a_fBase, double a_fExp)
        {
            return Math.Pow(a_fBase, a_fExp);
        }

        public static float powf(float a_fBase, float a_fExp)
        {
            return MathF.Pow(a_fBase, a_fExp);
        }

        public static float sqrt(float a_fArg)
        {
            return MathF.Sqrt(a_fArg);
        }

        public static float sqrtf(float a_fArg)
        {
            return MathF.Sqrt(a_fArg);
        }

        public static double sqrt(double a_fArg)
        {
            return Math.Sqrt(a_fArg);
        }

        public static float ceil(float a_fArg)
        {
            return MathF.Ceiling(a_fArg);
        }

        public static float ceilf(float a_fArg)
        {
            return MathF.Ceiling(a_fArg);
        }

        public static double ceil(double a_fArg)
        {
            return Math.Ceiling(a_fArg);
        }

        public static decimal ceil(decimal a_fArg)
        {
            return Math.Ceiling(a_fArg);
        }

        public static decimal ceill(decimal a_fArg)
        {
            return Math.Ceiling(a_fArg);
        }

        public static float floor(float a_fArg)
        {
            return MathF.Floor(a_fArg);
        }

        public static float floorf(float a_fArg)
        {
            return MathF.Floor(a_fArg);
        }

        public static double floor(double a_fArg)
        {
            return Math.Floor(a_fArg);
        }

        public static decimal floor(decimal a_fArg)
        {
            return Math.Floor(a_fArg);
        }

        public static decimal floorl(decimal a_fArg)
        {
            return Math.Floor(a_fArg);
        }

        public static float round(float a_fArg)
        {
            return MathF.Round(a_fArg);
        }

        public static float roundf(float a_fArg)
        {
            return MathF.Round(a_fArg);
        }

        public static double round(double a_fArg)
        {
            return Math.Round(a_fArg);
        }

        public static decimal round(decimal a_fArg)
        {
            return Math.Round(a_fArg);
        }

        public static decimal roundl(decimal a_fArg)
        {
            return Math.Round(a_fArg);
        }

        public static int lround(float a_fArg)
        {
            return (static_cast_int)(MathF.Round(a_fArg));
        }

        public static int lroundf(float a_fArg)
        {
            return (static_cast_int)(MathF.Round(a_fArg));
        }

        public static int lround(double a_fArg)
        {
            return (static_cast_int)(Math.Round(a_fArg));
        }

        public static int lround(decimal a_fArg)
        {
            return (static_cast_int)(Math.Round(a_fArg));
        }

        public static int lroundl(decimal a_fArg)
        {
            return (static_cast_int)(Math.Round(a_fArg));
        }

        public static long llround(float a_fArg)
        {
            return (static_cast_long)(MathF.Round(a_fArg));
        }

        public static long llroundf(float a_fArg)
        {
            return (static_cast_long)(MathF.Round(a_fArg));
        }

        public static long llround(double a_fArg)
        {
            return (static_cast_long)(Math.Round(a_fArg));
        }

        public static long llround(decimal a_fArg)
        {
            return (static_cast_long)(Math.Round(a_fArg));
        }

        public static long llroundl(decimal a_fArg)
        {
            return (static_cast_long)(Math.Round(a_fArg));
        }
    }
}
