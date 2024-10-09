using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DrawCar.Utils
{
    public static class TransformExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FixCenterPivot(this Transform parent, out GameObject tempPivotObject)
        {
            tempPivotObject = new GameObject("TempPivot");
            Transform tempPivot = tempPivotObject.transform;
            List<Transform> childrenList = new();
            
            for (int i = 0; i < parent.childCount; i++)
            {
                childrenList.Add(parent.GetChild(i));
            }
            Vector3 centerPoint = Vector3.zero;
            foreach (Transform obj in childrenList)
            {
                centerPoint += obj.position;
            }
            
            centerPoint /= childrenList.Count;
            
            tempPivot.position = centerPoint;
            foreach (Transform obj in childrenList)
            {
                obj.SetParent(tempPivot);
            }
            
            parent.transform.position = tempPivot.position;
            SetChildrenList(in tempPivot, ref childrenList);
            foreach (Transform obj in childrenList)
            {
                obj.SetParent(parent.transform);
            }
            
            childrenList.Clear();
            childrenList = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetChildrenList(in Transform parent, ref List<Transform> childrenList)
        {
            childrenList.Clear();
            for (int i = 0; i < parent.childCount; i++)
            {
                childrenList.Add(parent.GetChild(i));
            }
        }
    }
}