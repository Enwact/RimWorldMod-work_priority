using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

[HarmonyPatch(typeof(Pawn_WorkSettings), nameof(Pawn_WorkSettings.SetPriority))]
public static class Patch_SetPriority
{
    static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);

        for (int i = 0; i < codes.Count - 1; i++)
        {
            // 找到 priority > 4 的比较
            if (codes[i].opcode == OpCodes.Ldc_I4_4 &&
                codes[i + 1].opcode == OpCodes.Ble)
            {
                codes[i].opcode = OpCodes.Ldc_I4;
                codes[i].operand = 10;

                //Log.Message("[Priority10] SetPriority upper bound changed to 10");
            }
        }

        return codes;
    }
}