using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    public class DarkLatexCubBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.DarkLatexCubBanner>());
            Item.rare = ItemRarityID.Blue;
        }
    }
}
