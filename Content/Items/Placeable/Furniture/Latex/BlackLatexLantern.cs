using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture.Latex
{
    public class BlackLatexLantern : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Latex.BlackLatexLantern>());
            Item.value = Item.sellPrice(copper: 30);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BlackGoo>(), 5)
                .AddIngredient(ModContent.ItemType<CrystalRed>(), 1)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
}
