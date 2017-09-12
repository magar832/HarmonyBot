using System;

namespace HarmonyBot
{
    class RndNumGen
    {
        public static Random rng = new Random();
        public static int Roll(int max)
        {
            return rng.Next(1, max+1);
        }
    }
}
