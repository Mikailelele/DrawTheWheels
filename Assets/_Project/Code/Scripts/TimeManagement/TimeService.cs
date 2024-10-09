using System.Runtime.CompilerServices;
using UnityEngine;

namespace Code.Scripts.TimeManagement
{
    public sealed class TimeService
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPause(bool value)
        {
            Time.timeScale = value ? 0 : 1;
        }
    }
}