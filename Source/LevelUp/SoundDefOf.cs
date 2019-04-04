using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace LevelUp
{
    [DefOf]
    public static class LevellingSoundDefOf
    {
        // Token: 0x0600377F RID: 14207 RVA: 0x001A83FF File Offset: 0x001A67FF
        static LevellingSoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
        }
        public static SoundDef Level_Up;
    }
}
