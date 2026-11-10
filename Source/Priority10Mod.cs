using System.Reflection;
using HarmonyLib;
using Verse;

namespace Priority10
{
    public class Priority10Mod : Mod
    {
        public Priority10Mod(ModContentPack content)
            : base(content)
        {
            Harmony harmony = new Harmony("Enwact.Priority10");

            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Log.Message("[Priority10] Harmony patches loaded.");
        }
    }
}