using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Summons
{
    public class SummonWolfKing : ModItem
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
            var result = ChangedUtils.WolfKingSpawnCheck(true);
            return true;
            /*
            if (NPC.AnyNPCs(ModContent.NPCType<WolfKingSpawn>()) || NPC.AnyNPCs(ModContent.NPCType<WolfKing>()))
                return true;

            int blockCheckSpacing = 8;

            for (int y = 0; y < Main.worldSurface; y += blockCheckSpacing)
            {
                for (int x = 0; x < Main.maxTilesX; x += blockCheckSpacing)
                {
                    if (x < Main.maxTilesX)
                    {
                        var tile = Main.tile[x, y];
                        if (ChangedUtils.IsBlackLatexWall(tile))
                        {
                            player.position = new Vector2(x * 16, y * 16);
                            SoundEngine.PlaySound(SoundID.NPCDeath64, player.Center);
                            ChangedUtils.SpawnWolfKing(x, y, player);
                            return true;
                        }
                    }
                }
            }

            return false;
            */
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ModContent.ItemType<CrystalRed>(), 2)
                .AddIngredient(ModContent.ItemType<CrystalGreen>(), 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}