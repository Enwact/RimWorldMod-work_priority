
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Priority10
{
    [HarmonyPatch(typeof(WidgetsWork), nameof(WidgetsWork.DrawWorkBoxFor))]
    public static class Patch_MouseWheelPriority
    {
        private const int MaxPriority = 10;

        public static void Prefix(
            float x,
            float y,
            Pawn p,
            WorkTypeDef wType,
            bool incapableBecauseOfCapacities)
        {
            // 只在手动优先级模式下工作
            if (!Find.PlaySettings.useWorkPriorities)
                return;

            // 排除禁用工作类型
            if (p.WorkTypeIsDisabled(wType))
                return;

            // 工作格子的原版尺寸为 25x25
            Rect rect = new Rect(x, y, 25f, 25f);

            if (!Mouse.IsOver(rect))
                return;

            Event current = Event.current;

            if (current.type != EventType.ScrollWheel)
                return;

            int priority = p.workSettings.GetPriority(wType);

            if (current.delta.y < 0)
            {
                // 滚轮向上：提高优先级
                priority--;

                if (priority < 0)
                    priority = MaxPriority;
            }
            else if (current.delta.y > 0)
            {
                // 滚轮向下：降低优先级
                priority++;

                if (priority > MaxPriority)
                    priority = 0;
            }

            p.workSettings.SetPriority(wType, priority);

            current.Use();
        }
    }
}
