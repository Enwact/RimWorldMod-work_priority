using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Priority10
{
    [HarmonyPatch(
        typeof(PawnColumnWorker_WorkPriority),
        "HeaderClicked"
    )]
    public static class Patch_HeaderClicked
    {
        static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();

            int left = 0;
            int right = 0;

            for (int i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode != OpCodes.Ldc_I4_4)
                    continue;

                // num = 4
                if (IsStoreLocal(codes[i + 1]))
                {
                    codes[i].opcode = OpCodes.Ldc_I4;
                    codes[i].operand = 10;
                    left++;
                }

                // if (num2 > 4)
                if (i > 0 &&
                    IsLoadLocal(codes[i - 1]) &&
                    (codes[i + 1].opcode == OpCodes.Ble ||
                     codes[i + 1].opcode == OpCodes.Ble_S))
                {
                    codes[i].opcode = OpCodes.Ldc_I4;
                    codes[i].operand = 10;
                    right++;
                }
            }

            //Log.Message(
            //    $"[Priority10] HeaderClicked: left={left}, right={right}"
            //);

            if (left != 1 || right != 1)
            {
                throw new System.Exception(
                    "Unexpected HeaderClicked IL structure!"
                );
            }

            return codes;
        }

        private static bool IsStoreLocal(CodeInstruction c)
        {
            return c.opcode == OpCodes.Stloc ||
                   c.opcode == OpCodes.Stloc_S ||
                   c.opcode == OpCodes.Stloc_0 ||
                   c.opcode == OpCodes.Stloc_1 ||
                   c.opcode == OpCodes.Stloc_2 ||
                   c.opcode == OpCodes.Stloc_3;
        }

        private static bool IsLoadLocal(CodeInstruction c)
        {
            return c.opcode == OpCodes.Ldloc ||
                   c.opcode == OpCodes.Ldloc_S ||
                   c.opcode == OpCodes.Ldloc_0 ||
                   c.opcode == OpCodes.Ldloc_1 ||
                   c.opcode == OpCodes.Ldloc_2 ||
                   c.opcode == OpCodes.Ldloc_3;
        }
    }
}