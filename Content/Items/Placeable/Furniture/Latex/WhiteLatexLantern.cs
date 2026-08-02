using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture.Latex
{
    public class WhiteLatexLantern : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Latex.WhiteLatexLantern>());
            Item.value = Item.sellPrice(copper: 30);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WhiteGoo>(), 5)
                .AddIngredient(ModContent.ItemType<CrystalWhite>(), 1)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
}
