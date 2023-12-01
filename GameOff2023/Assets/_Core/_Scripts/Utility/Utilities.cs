using System.Collections.Generic;
using UnityEngine;

namespace _Core._Scripts.Utility
{
    public static class Utilities
    {
        /// <summary>
        /// Used to shuffle lists to support RNG.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleList<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}