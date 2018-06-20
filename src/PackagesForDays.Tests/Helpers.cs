using System;
using System.Collections.Generic;
using System.Text;

namespace PackagesForDays.Tests
{
    static class Helpers
    {
        private static Random rng = new Random();

        //https://stackoverflow.com/questions/273313/randomize-a-listt
        //shuffle a list helper
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
