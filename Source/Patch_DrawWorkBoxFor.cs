
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace Priority10
{
    [HarmonyPatch(typeof(WidgetsWork), nameof(WidgetsWork.DrawWorkBoxFor))]
    public static class Patch_DrawWorkBoxFor
    {
        static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            int replacedLeft = 0;
            int replacedRight = 0;

            for (int i = 0; i < codes.Count - 1; i++)
            {
                // 左键：num = 4
                // 原版结构：ldc.i4.4 -> stloc
                if (codes[i].opcode == OpCodes.Ldc_I4_4 &&
                    IsStoreLocal(codes[i + 1]))
                {
                    codes[i].opcode = OpCodes.Ldc_I4;
                    codes[i].operand = 10;
                    replacedLeft++;
                }

                // 右键：if (num2 > 4)
                // 原版结构：ldloc -> ldc.i4.4 -> ble.s
                if (codes[i].opcode == OpCodes.Ldc_I4_4 &&
                    i > 0 &&
                    IsLoadLocal(codes[i - 1]) &&
                    i + 1 < codes.Count &&
                    (codes[i + 1].opcode == OpCodes.Ble ||
                     codes[i + 1].opcode == OpCodes.Ble_S))
                {
                    codes[i].opcode = OpCodes.Ldc_I4;
                    codes[i].operand = 10;
                    replacedRight++;
                }
            }

            //Log.Message(
            //    $"[Priority10] DrawWorkBoxFor: " +
            //    $"left={replacedLeft}, right={replacedRight}"
            //);

            if (replacedLeft != 1 || replacedRight != 1)
            {
                throw new System.Exception(
                    "Priority10: Unexpected DrawWorkBoxFor IL structure!"
                );
            }

            return codes;
        }

        private static bool IsStoreLocal(CodeInstruction instruction)
        {
            return instruction.opcode == OpCodes.Stloc ||
                   instruction.opcode == OpCodes.Stloc_S ||
                   instruction.opcode == OpCodes.Stloc_0 ||
                   instruction.opcode == OpCodes.Stloc_1 ||
                   instruction.opcode == OpCodes.Stloc_2 ||
                   instruction.opcode == OpCodes.Stloc_3;
        }

        private static bool IsLoadLocal(CodeInstruction instruction)
        {
            return instruction.opcode == OpCodes.Ldloc ||
                   instruction.opcode == OpCodes.Ldloc_S ||
                   instruction.opcode == OpCodes.Ldloc_0 ||
                   instruction.opcode == OpCodes.Ldloc_1 ||
                   instruction.opcode == OpCodes.Ldloc_2 ||
                   instruction.opcode == OpCodes.Ldloc_3;
        }
    }
}
