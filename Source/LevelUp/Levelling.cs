using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    public class HealthLevelling : ThingComp
    {
        LevelUpModSettings settings = LoadedModManager.GetMod<LevelUpMod>().GetSettings<LevelUpModSettings>();
        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (parent is Pawn pawn)
            {
                if (pawn.health.hediffSet.HasHediff(LevellingHediffDefOf.HealthLevel))
                {
                    float HealthScale = pawn.health.hediffSet.GetFirstHediffOfDef(LevellingHediffDefOf.HealthLevel).Severity;
                    int LevellingSeverity = Mathf.FloorToInt(HealthScale);
                    //float RemainingTillNextLevel = (Mathf.CeilToInt(HealthScale) - (HealthScale)) * 100;
                    //float RemainingTillNextLevelIncremental = RemainingTillNextLevel * LevellingSeverity;
                    //Log.Message("Pre Calculation: " + RemainingTillNextLevel + " - " + totalDamageDealt + " = " + (RemainingTillNextLevel - totalDamageDealt));
                    //Log.Message("Actual Calculation: " + RemainingTillNextLevelIncremental + " - " + totalDamageDealt + " = " + (RemainingTillNextLevelIncremental - totalDamageDealt));
                    float Compound = (75 * Mathf.Pow(settings.LevelUpRate + 1, LevellingSeverity)) * pawn.RaceProps.baseHealthScale;
                    if (pawn.Faction != null)
                    {
                        if ((HealthScale - LevellingSeverity) + totalDamageDealt / Compound >= 1 && pawn.Faction.IsPlayer)
                        {
                            Messages.Message(pawn.Name.ToStringShort + " has leveled up!", pawn, MessageTypeDefOf.SilentInput);
                            LevellingSoundDefOf.Level_Up.PlayOneShotOnCamera(null);
                        }
                    }
                    //Log.Message(totalDamageDealt + " / " + "75(1.075)^" + LevellingSeverity + " = " + totalDamageDealt / Compound * 75);
                    HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, totalDamageDealt / Compound);
                }
                else
                {
                    HediffMaker.MakeHediff(LevellingHediffDefOf.HealthLevel, pawn);
                    HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, totalDamageDealt / (75 * pawn.RaceProps.baseHealthScale));
                }
            }
        }
        public override void PostSpawnSetup(bool respawnAfterLoad)
        {
            base.PostSpawnSetup(respawnAfterLoad);
            if (parent != null)
            {
                if (parent is Pawn pawn)
                {
                    float Severity = Rand.Range(0f, 10f);
                    bool HealthHediff = pawn.health.hediffSet.HasHediff(LevellingHediffDefOf.HealthLevel);
                    if (pawn.kindDef.defaultFactionType != null)
                    {
                        bool Player = pawn.kindDef.defaultFactionType.isPlayer;
                        if (Player && !HealthHediff)
                        {
                            HediffMaker.MakeHediff(LevellingHediffDefOf.HealthLevel, pawn);
                            HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, 0.0001f);
                            return;
                        }
                        else if (!HealthHediff && !Player)
                        {
                            HediffMaker.MakeHediff(LevellingHediffDefOf.HealthLevel, pawn);
                            HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, Severity / pawn.RaceProps.baseHealthScale);
                            return;
                        }
                    }
                    if (pawn.kindDef.defaultFactionType == null && !HealthHediff)
                    {
                        HediffMaker.MakeHediff(LevellingHediffDefOf.HealthLevel, pawn);
                        if (settings.LevelUpBodySizeMattersLess == true)
                        {
                            if (pawn.RaceProps.baseHealthScale <= 1)
                            {
                                HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, Severity);
                            }
                            else if (pawn.RaceProps.baseHealthScale > 1)
                            {
                                HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, Severity / pawn.RaceProps.baseHealthScale);
                            }
                        }
                        else
                        {
                            HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, Severity / pawn.RaceProps.baseHealthScale);
                        }
                    }
                }
                return;
            }
            return;
        }
    }
}
