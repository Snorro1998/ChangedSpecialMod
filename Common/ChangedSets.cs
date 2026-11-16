using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common
{
    [ReinitializeDuringResizeArrays]
    public class ChangedSets : ModSystem
    {
        internal static SetFactory NPCFactory = new(NPCLoader.NPCCount, "ChangedNPCs");

        /// <summary> If this NPC can meow when hurt or killed. </summary>
        public static bool[] CanMeowOnHurt = NPCFactory.CreateBoolSet();

        /// <summary> What the NPC can evolve into by merging with one of his kind. </summary>
        public static int[] EvolveType = NPCFactory.CreateIntSet(NPCID.None);
    }
}
