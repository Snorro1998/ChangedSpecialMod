using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Common.Systems.ModSupport;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 1;
            Item.useTime = 1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                var npc = Main.npc[i];
                if (npc == null || !npc.active || npc.aiStyle != NPCAIStyleID.Passive)
                    continue;

                // Makes all town npcs blink
                npc.ai[0] = 2;
                npc.ai[1] = 45 * Main.rand.Next(1, 2);
                npc.ai[2] = 0;
                npc.ai[3] = 0;

                npc.localAI[0] = 0;
                npc.localAI[1] = 0;
                npc.localAI[2] = 0;
                npc.localAI[3] = 0;

            }

            return true;
        }
    }
}