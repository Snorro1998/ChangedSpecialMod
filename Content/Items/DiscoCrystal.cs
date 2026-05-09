using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class DiscoCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CrystalRed>(), 3)
                .AddIngredient(ModContent.ItemType<CrystalGreen>(), 3)
                .AddIngredient(ModContent.ItemType<CrystalWhite>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}