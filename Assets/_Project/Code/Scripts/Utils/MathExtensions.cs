using System;
using System.Runtime.CompilerServices;

namespace DrawCar.Utils
{
    public static class MathExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NextOf<T>(this T[] array, in T item)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Array is null or empty.");
            }

            var indexOf = Array.IndexOf(array, item);

            if (indexOf == -1)
            {
                throw new ArgumentException("Item not found in the array.");
            }

            var nextIndex = (indexOf + 1) % array.Length;
            return array[nextIndex];
        }
    }
}