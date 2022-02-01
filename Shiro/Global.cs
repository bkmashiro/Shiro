
global using static System.Math;
global using static Shiro.Vectors;
global using static Shiro.GLOBAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Shiro
{
    public static class GLOBAL{
        public static readonly float EPSILON = 0.00001F;
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        static Random random = new Random();
        public static float get_random_float()
        {
            return random.NextSingle();
        }
    }
}
