
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Priority10
{
    [HarmonyPatch(
        typeof(Pawn_WorkSettings),
        nameof(Pawn_WorkSettings.SetPriority)
    )]
    public static class Patch_SetPriorityValidation
    {
        static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            int replaced = 0;

            for (int i = 0; i < codes.Count - 1; i++)
            {
                // 定位 priority > 4 的校验
                if (codes[i].opcode == OpCodes.Ldc_I4_4 &&
                    (codes[i + 1].opcode == OpCodes.Ble ||
                     codes[i + 1].opcode == OpCodes.Ble_S))
                {
                    codes[i].opcode = OpCodes.Ldc_I4;
                    codes[i].operand = 10;
                    replaced++;
                }
            }

            //Log.Message(
            //    $"[Priority10] SetPriority validation replaced={replaced}"
            //);

            if (replaced != 1)
            {
                throw new Exception(
                    "Priority10: Unexpected SetPriority IL structure!"
                );
            }

            return codes;
        }
    }
}
