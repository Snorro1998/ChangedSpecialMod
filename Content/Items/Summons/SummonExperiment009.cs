using ChangedSpecialMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Summons
{
    public class SummonExperiment009 : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            int bossType = ModContent.NPCType<Experiment009>();

            if (NPC.AnyNPCs(bossType))
                return true;

            int targetTileX = (int)player.Center.X;
            int targetTileY = (int)player.Center.Y;

            int npcIndex = NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), 0, 0, bossType);
            if (npcIndex > 0)
            {
                var bossNPC = Main.npc[npcIndex];
                bossNPC.position.X = targetTileX + 6 * bossNPC.width / 2;
                bossNPC.position.Y = targetTileY - bossNPC.height;
            }

            return true;
        }
    }
}