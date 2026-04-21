using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    public class WhiteLatexBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.WhiteLatexBanner>());
        }
    }
}
