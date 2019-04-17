using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace LevelUp
{
    public class LevelUpModSettings : ModSettings
    {
        public bool LevelUpBodySizeMattersLess;
        public float LevelUpRate = 0.075f;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref LevelUpBodySizeMattersLess, "BodySizeMattersLess");
            Scribe_Values.Look(ref LevelUpRate, "LevellingRate", 0.075f);
            base.ExposeData();
        }
    }
    public class LevelUpMod : Mod
    {
        LevelUpModSettings settings;
        public LevelUpMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<LevelUpModSettings>();
        }
        public override string SettingsCategory() => "Level This!";
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Body Size Matters Less", ref settings.LevelUpBodySizeMattersLess, "Ignores the body health scales below one (Rats, Squirrels, etc.) in randomising animal levels, this will make smaller animals less OP.");
            listingStandard.Label($"Levelling Rate ({settings.LevelUpRate.ToStringPercent()})");
            settings.LevelUpRate = listingStandard.Slider(settings.LevelUpRate, 0.001f, 1f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }
    }
}