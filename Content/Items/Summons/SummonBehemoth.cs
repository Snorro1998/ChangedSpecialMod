using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Summons
{
    public class SummonBehemoth : ModItem
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
            int bossType = ModContent.NPCType<Behemoth>();

            if (NPC.AnyNPCs(bossType))
                return true;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int targetTileX = (int)(player.Center.X / 16f);
                int targetTileY = (int)(player.Center.Y / 16f);
                Vector2 chosenTile = Vector2.Zero;

                int npcIndex = NPC.NewNPC(
                    NPC.GetBossSpawnSource(player.whoAmI),
                    0,
                    0,
                    bossType
                );

                if (npcIndex >= 0)
                {
                    NPC bossNPC = Main.npc[npcIndex];

                    if (!bossNPC.AI_AttemptToFindTeleportSpot(
                        ref chosenTile,
                        targetTileX,
                        targetTileY,
                        20,
                        5,
                        1,
                        false,
                        false))
                    {
                        chosenTile = new Vector2(targetTileX, targetTileY);
                    }

                    bossNPC.position.X = chosenTile.X * 16f - bossNPC.width / 2f;
                    bossNPC.position.Y = chosenTile.Y * 16f - bossNPC.height;

                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, number: npcIndex);
                    }
                }
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Gel, 20)
                .AddIngredient(ModContent.ItemType<PillarWhite>(), 3)
                .AddIngredient(ModContent.ItemType<CrystalWhite>(), 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}