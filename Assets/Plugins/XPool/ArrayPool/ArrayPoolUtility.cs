﻿using System;
using UnityEngine.Profiling;

namespace XPool
{
    public static class ArrayPoolUtility
    {
        public const int kMinArraySize = 8;
        public static void EnsureFixedCapacity<T>(ref T[] array, int fixedSize, ArrayPool<T> pool)
        {
            var preLen = array == null ? 0 : array.Length;
            if (preLen <= fixedSize)
            {
                int minimumSize = (preLen != 0) ? preLen * 2 : kMinArraySize;
                Profiler.BeginSample("ArrayPoolUtility.EnsureFixedCapacity");
                T[] newArray = pool.Rent((fixedSize < minimumSize) ? minimumSize : fixedSize);
                Profiler.EndSample();
                if (array != null)
                {
                    Array.Copy(array, 0, newArray, 0, array.Length);
                    pool.Return(array, !RuntimeHelpers.IsWellKnownNoReferenceContainsType<T>());
                }

                array = newArray;
            }
        }
    }
}