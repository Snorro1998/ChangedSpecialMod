using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    public class WhiteLatexTaurBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.WhiteLatexTaurBanner>());
            Item.rare = ItemRarityID.Blue;
        }
    }
}
