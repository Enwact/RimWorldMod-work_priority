
using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace Priority10
{
    [HarmonyPatch(
        typeof(WidgetsWork),
        nameof(WidgetsWork.ColorOfPriority)
    )]
    public static class Patch_ColorOfPriority
    {
        private static readonly Color[] PriorityColors =
        {
            Color.clear, // 0：不显示

            new Color(0.00f, 1.00f, 0.00f), // 1 原版绿色
            new Color(0.33f, 0.97f, 0.17f), // 2
            new Color(0.67f, 0.93f, 0.33f), // 3
            new Color(1.00f, 0.90f, 0.50f), // 4 原版黄色

            new Color(0.93f, 0.83f, 0.50f), // 5
            new Color(0.87f, 0.77f, 0.50f), // 6
            new Color(0.80f, 0.70f, 0.50f), // 7 原版棕黄色

            new Color(0.78f, 0.71f, 0.58f), // 8
            new Color(0.76f, 0.73f, 0.66f), // 9
            new Color(0.74f, 0.74f, 0.74f)  // 10 原版灰色
        };

        static bool Prefix(int prio, ref Color __result)
        {
            if (prio >= 1 && prio <= 10)
            {
                __result = PriorityColors[prio];
            }
            else
            {
                __result = Color.grey;
            }

            return false;
        }
    }
}
