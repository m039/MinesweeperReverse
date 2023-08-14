using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
    public static class CollectionExt
    {
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                var destination = Random.Range(0, i + 1);
                var temp = list[i];
                list[i] = list[destination];
                list[destination] = temp;
            }
        }
    }
}
