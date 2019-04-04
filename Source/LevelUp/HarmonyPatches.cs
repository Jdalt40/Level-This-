using Harmony;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace LevelUp
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("Jdalt.RimWorld.LevelUp.Main");
            harmony.Patch(AccessTools.Property(typeof(Pawn), nameof(Pawn.HealthScale)).GetGetMethod(), null, new HarmonyMethod(typeof(HarmonyPatches), nameof(LevelUpHealthScale)));
        }
        public static void LevelUpHealthScale(Pawn __instance, ref float __result)
        {
            if (__instance.health.hediffSet.HasHediff(LevellingHediffDefOf.HealthLevel))
            {
                int Level = Mathf.FloorToInt(__instance.health.hediffSet.GetFirstHediffOfDef(LevellingHediffDefOf.HealthLevel).Severity);
                //Log.Message("Compounding Health Maths: " + __instance.RaceProps.baseHealthScale + "(1.1f)^" + Level + " = " + __instance.RaceProps.baseHealthScale * (Mathf.Pow(1.1f, Level)), true);
                __result *= (1 * Mathf.Pow(1.1f, Level));
            }
        }
    }
}
