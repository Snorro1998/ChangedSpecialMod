using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.NPCs.Drunk;
using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugSpawnAllBosses : ModItem
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
            var entitySource = new EntitySource_SpawnNPC("ChangedUtils");
            var nNPCTypes = Main.npcFrameCount.Length;
            for (int i = 0; i < nNPCTypes; i++)
            {
                if (NPC.AnyNPCs(i))
                    continue;

                if (player == null)
                    player = Main.player.Where(x => x.active).FirstOrDefault();
                if (player == null)
                    return true;

                int xPos = (int)player.Center.X + Main.rand.Next(-40, 40) * 16;
                int yPos = (int)player.Center.Y + Main.rand.Next(-20, 0) * 16;
                var npc = NPC.NewNPCDirect(entitySource, xPos, yPos, i, player.whoAmI);

                if (!npc.boss)
                    npc.active = false;
            }

            return true;
        }
    }
}